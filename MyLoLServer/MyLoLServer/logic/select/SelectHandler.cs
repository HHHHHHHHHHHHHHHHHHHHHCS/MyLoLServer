using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.auto;
using MyLoLServer.tool;
using GameProtocol;
using System.Collections.Concurrent;

namespace MyLoLServer.logic.select
{
    public class SelectHandler : AbsOnceHandler, InterfaceHandler
    {

        public override byte Type
        {
            get
            {
                return Protocol.TYPE_SELECT;
            }
        }

        public override int Area
        {
            get
            {
                return SelectProtocol.AREA_SELECT;
            }
        }

        /// <summary>
        /// 多线程处理类中 防止数据竞争导致脏数据   使用线程安全字典
        /// 玩家所在匹配房间映射   玩家ID 房间ID
        /// </summary>
        ConcurrentDictionary<string, int> userRoom = new ConcurrentDictionary<string, int>();


        /// <summary>
        /// 房间ID 与模型映射
        /// </summary>
        ConcurrentDictionary<int, SelectRoom> roomMap = new ConcurrentDictionary<int, SelectRoom>();


        /// <summary>
        /// 回收利用过的房间对象再次利用，减少gc性能开销
        /// </summary>
        ConcurrentQueue<SelectRoom> cache = new ConcurrentQueue<SelectRoom>();


        /// <summary>
        /// 用于房间ID 
        /// </summary>
        ConcurrentIntegerID index = new ConcurrentIntegerID();



        public SelectHandler()
        {
            EventUtil.createSelect = Create;
            EventUtil.destroySelect = Destroy;
        }

        public void Create(List<string> teamOne, List<string> teamTwo)
        {
            SelectRoom room;
            if (!cache.TryDequeue(out room))
            {
                room = new SelectRoom();
                //添加唯一房间ID
                room.roomID = index.GetAndAdd();
            }

            //房间数据初始化
            room.Init(teamOne, teamTwo);
            //绑定映射关系
            foreach (string item in teamOne)
            {
                userRoom.TryAdd(item, room.roomID);

            }
            foreach (string item in teamTwo)
            {
                userRoom.TryAdd(item, room.roomID);

            }
            roomMap.TryAdd(room.roomID, room);
        }

        public void Destroy(int roomID)
        {
            SelectRoom room;
            if (roomMap.TryRemove(roomID, out room))
            {//移除角色和房间之间的绑定关系
                int temp = 0;
                foreach (string item in room.teamOne.Keys)
                {
                    userRoom.TryRemove(item, out temp);
                }
                foreach (string item in room.teamTwo.Keys)
                {
                    userRoom.TryRemove(item, out temp);
                }

                //将房间丢进缓存队列 供下次选择使用
                cache.Enqueue(room);
            }
        }


        public void ClientClose(UserToken token, string error)
        {
            string userID = GetUserID(token);
            //判断当前玩家是否有房间
            if (userRoom.ContainsKey(userID))
            {
                int roomID;
                //移除并获取玩家所在房间
                userRoom.TryRemove(userID, out roomID);
                if (roomMap.ContainsKey(roomID))
                {
                    //通知
                    roomMap[roomID].ClientClose(token, error);
                }
            }
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            //转发消息 :两种方法二选一
            /*if(roomMap.ContainsKey(message.area))
            {//前提是区域码就是 房间ID
                roomMap[message.area].MessageReceive(token, message);
            }*/

            string userID = GetUserID(token);
            if (userRoom.ContainsKey(userID))
            {
                int roomID = userRoom[userID];
                if (roomMap.ContainsKey(roomID))
                {
                    roomMap[roomID].MessageReceive(token, message);
                }
            }
        }
    }
}
