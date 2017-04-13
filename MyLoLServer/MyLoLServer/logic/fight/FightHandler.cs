using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.auto;
using GameProtocol;
using System.Collections.Concurrent;
using MyLoLServer.tool;
using GameProtocol.dto;

namespace MyLoLServer.logic.fight
{
    class FightHandler : AbsMulitHandlr, InterfaceHandler
    {
        public override byte Type
        {
            get
            {
                return Protocol.TYPE_FIGHT;
            }
        }

        /// <summary>
        /// 多线程处理类中 防止数据竞争导致脏数据 使用线程安全字典
        /// 玩家所在战斗房间映射
        /// </summary>
        ConcurrentDictionary<string, int> userRoom = new ConcurrentDictionary<string, int>();

        /// <summary>
        /// 房间ID与模型的映射
        /// </summary>
        ConcurrentDictionary<int, FightRoom> roomMap = new ConcurrentDictionary<int, FightRoom>();


        /// <summary>
        /// 回收利用过的房间对象再次利用 减少GC的性能开销
        /// </summary>
        ConcurrentQueue<FightRoom> cache = new ConcurrentQueue<FightRoom>();


        /// <summary>
        /// 房间ID自增器
        /// </summary>
        ConcurrentIntegerID index = new ConcurrentIntegerID();

        public FightHandler()
        {
            EventUtil.createFight = Create;
            EventUtil.destroyFight = Destroy;
        }

        /// <summary>
        /// 创建战斗房间
        /// </summary>
        /// <param name="teamOne"></param>
        /// <param name="teamTwo"></param>
        public void Create(DTOSelectModel[] teamOne, DTOSelectModel[] teamTwo)
        {
            FightRoom room;
            if (!cache.TryDequeue(out room))
            {
                room = new FightRoom();
                //添加唯一ID
                room.Area = index.GetAndAdd();
            }


            //房间数据初始化
            room.Init(teamOne, teamTwo);

            foreach (DTOSelectModel item in teamOne)
            {
                userRoom.TryAdd(item.userID, room.Area);
            }

            foreach (DTOSelectModel item in teamTwo)
            {
                userRoom.TryAdd(item.userID, room.Area);
            }

            roomMap.TryAdd(room.Area, room);
        }

        /// <summary>
        /// 战斗结束房间移除
        /// </summary>
        /// <param name="roomID"></param>
        public void Destroy(int roomID)
        {
            FightRoom room;
            if (roomMap.TryRemove(roomID, out room))
            {//移除角色和房间之间的绑定关系
                int temp = 0;
                foreach (AbsFightModel item in room.teamOne.Values)
                {
                    userRoom.TryRemove(((DTOFightPlayerModel)item).userID, out temp);
                }
                foreach (AbsFightModel item in room.teamTwo.Values)
                {
                    userRoom.TryRemove(((DTOFightPlayerModel)item).userID, out temp);
                }
                room.Destroy();
                //将房间丢进缓存队列 供下次选择使用
                cache.Enqueue(room);
            }
        }

        public void ClientClose(UserToken token, string error)
        {
            //判断玩家是否在某场战斗中
            if(userRoom.ContainsKey(GetUserID(token)))
            {
                Console.WriteLine("CLEAR"+ roomMap[userRoom[GetUserID(token)]]==null?"N":"Y");
                roomMap[userRoom[GetUserID(token)]].ClientClose(token, error);
            }
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            roomMap[userRoom[GetUserID(token)]].MessageReceive(token, message);
        }
    }
}
