using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.auto;
using GameProtocol;
using MyLoLServer.dao.model;
using System.Collections.Concurrent;
using MyLoLServer.tool;

namespace MyLoLServer.logic.match
{
    /// <summary>
    /// 战斗匹配逻辑处理类
    /// </summary>
    public class MatchHandler : AbsOnceHandler, InterfaceHandler
    {
        public override byte Type
        {
            get
            {
                return Protocol.TYPE_MATCH;
            }
        }

        public override int Area
        {
            get
            {
                return MatchProtocol.AREA_MATCH;
            }
        }

        /// <summary>
        /// 多线程处理类中 防止数据竞争导致脏数据   使用线程安全字典
        /// 玩家所在匹配房间映射
        /// </summary>
        ConcurrentDictionary<string, string> userRoom = new ConcurrentDictionary<string, string>();


        /// <summary>
        /// 房间ID 与模型映射
        /// </summary>
        ConcurrentDictionary<string, MatchRoom> roomMap = new ConcurrentDictionary<string, MatchRoom>();


        /// <summary>
        /// 回收利用过的房间对象再次利用，减少gc性能开销
        /// </summary>
        ConcurrentQueue<MatchRoom> cache = new ConcurrentQueue<MatchRoom>();


        /// <summary>
        /// 房间ID 使用
        /// </summary>
        ConcurrentStringID index = new ConcurrentStringID();

        public void ClientClose(UserToken token, string error)
        {
            LevelRoom(token);
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.command)
            {
                case MatchProtocol.ENTER_CREQ:
                    EnterMatch(token);
                    break;
                case MatchProtocol.LEAVE_CREQ:
                    LevelRoom(token);
                    break;
            }
        }

        private void EnterMatch(UserToken token)
        {
            string userID = GetUserID(token);
            //判断玩家当前是否正在匹配队列中
            if (!userRoom.ContainsKey(userID))
            {
                Console.WriteLine("用户开始匹配" + userID);
                MatchRoom room = null;
                bool isEnter = false;
                //当前是否有等待中的房间
                if (roomMap.Count > 0)
                {
                    foreach (MatchRoom item in roomMap.Values)
                    {
                        if (item.teamMax << 1 > item.teamOne.Count + item.teamTwo.Count)
                        {
                            room = item;
                            if (room.teamOne.Count < room.teamMax)
                            {
                                room.teamOne.Add(userID);
                            }
                            else
                            {
                                room.teamTwo.Add(userID);
                            }
                            isEnter = true;
                            //添加玩家与房间的映射关系 
                            userRoom.TryAdd(userID, room.id);
                            break;
                        }
                    }
                }
                if (room == null)
                {
                    if (!isEnter)
                    {
                        //如果还没有进入房间
                        //直接从缓存列表可用房间 或者创建新房间
                        if (cache.Count > 0)
                        {
                            cache.TryDequeue(out room);
                            room.teamOne.Add(userID);
                            roomMap.TryAdd(room.id, room);
                            userRoom.TryAdd(userID, room.id);
                        }
                        else
                        {
                            room = new MatchRoom(index.GetNewStringID());
                            room.teamOne.Add(userID);
                            roomMap.TryAdd(room.id, room);
                            userRoom.TryAdd(userID, room.id);
                        }
                    }
                }


                //不管什么方式进入房间，判断当前的房间是否满员
                //满了就开始选人并将当前的房间丢进缓存堆
                if (room.teamOne.Count == room.teamTwo.Count
                    && (room.teamMax << 1) == room.teamOne.Count + room.teamTwo.Count)
                {
                    //通知选人模块 开始选人了
                    EventUtil.createSelect(room.teamOne, room.teamTwo);
                    WriteToUsers(room.teamOne.ToArray(), Type, Area, MatchProtocol.ENTER_SELECT_BRO, MatchProtocol.TAG_TEAMONE);
                    WriteToUsers(room.teamTwo.ToArray(), Type, Area, MatchProtocol.ENTER_SELECT_BRO, MatchProtocol.TAG_TEAMTWO);

                    //移除玩家与房间的映射
                    string i;
                    foreach (string item in room.teamOne)
                    {
                        userRoom.TryRemove(item, out i);
                    }
                    foreach (string item in room.teamTwo)
                    {

                        userRoom.TryRemove(item, out i);
                    }
                    i = null;

                    //重置房间数据 供下次使用
                    room.teamOne.Clear();
                    room.teamTwo.Clear();
                    //将房间从等待房间表中移除
                    roomMap.TryRemove(room.id, out room);
                    //将房间丢进缓存表中 以供下次使用
                    cache.Enqueue(room);
                }
            }
        }

        private void LevelRoom(UserToken token)
        {
            //取出用户唯一ID
            string userID = GetUserID(token);
            //判断用户是否有房间映射关系
            if (!userRoom.ContainsKey(userID))
            {
                return;
            }
            Console.WriteLine("用户取消匹配" + userID);
            //获取用户所在房间ID
            string roomID = userRoom[userID];
            //判断是否拥有此房间
            if (roomMap.ContainsKey(roomID))
            {
                MatchRoom room = roomMap[roomID];
                //根据用户所在的队伍  进行移除
                if (room.teamOne.Contains(userID))
                {
                    room.teamOne.Remove(userID);
                }
                else
                {
                    room.teamTwo.Remove(userID);
                }
                //溢出用户与房间之间的映射关系
                userRoom.TryRemove(userID, out roomID);
                //如果当前用户所在的房间是最后一人，则移除房间放入缓存
                if (room.teamOne.Count + room.teamTwo.Count == 0)
                {
                    roomMap.TryRemove(roomID, out room);
                    cache.Enqueue(room);
                }
            }
        }
    }
}
