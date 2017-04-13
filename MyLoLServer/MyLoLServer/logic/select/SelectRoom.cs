using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.auto;
using System.Collections.Concurrent;
using GameProtocol.dto;
using MyLoLServer.tool;
using GameProtocol;
using MyLoLServer.dao.model;

namespace MyLoLServer.logic.select
{
    public class SelectRoom : AbsMulitHandlr, InterfaceHandler
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

        public int roomID { get; set; }

        public ConcurrentDictionary<string, DTOSelectModel> teamOne = new ConcurrentDictionary<string, DTOSelectModel>();
        public ConcurrentDictionary<string, DTOSelectModel> teamTwo = new ConcurrentDictionary<string, DTOSelectModel>();

        private List<string> readyList = new List<string>();

        int enterCount;//当前进入房间的人数
        string missionID;//当前定时任务ID

        public void Init(List<string> teamOneList, List<string> teamTwoList)
        {
            //重置数据
            Clear();

            foreach (string item in teamOneList)
            {
                DTOSelectModel select = new DTOSelectModel();
                select.userID = item;
                select.name = GetUser(item).name;
                select.hero = -1;
                select.isEnter = false;
                select.isReady = false;
                teamOne.TryAdd(item, select);
            }
            foreach (string item in teamTwoList)
            {
                DTOSelectModel select = new DTOSelectModel();
                select.userID = item;
                select.name = GetUser(item).name;
                select.hero = -1;
                select.isEnter = false;
                select.isReady = false;
                teamTwo.TryAdd(item, select);
            }


            //初始化完毕  开启定时任务  设定30秒后 没有进入到选择界面的时候 直接解散此次匹配
            ScheduleUtil.Instance.Schedule(EnterEvent, 30000);
        }

        public void ClientClose(UserToken token, string error)
        {
            //调用离开方法 让此连接 不再接受网络消息
            Leave(token);
            //通知房间其他人  房间解散了  回主界面去
            Brocast(SelectProtocol.DESTROY_BRO, null);
            EventUtil.destroySelect(roomID);
        }


        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.command)
            {
                case SelectProtocol.ENTER_CREQ:
                    {
                        EnterSelectRoom(token);
                        break;
                    }
                case SelectProtocol.SELECT_CREQ:
                    {
                        Select(token, message.GetMesssage<int>());
                        break;
                    }
                case SelectProtocol.TALK_CREQ:
                    {
                        AllTalk(token, message.GetMesssage<string>());
                        break;
                    }
                case SelectProtocol.READY_CREQ:
                    {
                        Ready(token);
                        break;
                    }
            }
        }

        private void Ready(UserToken token)
        {
            //判断玩家是否在房间里
            if (!IsEntered(token))
            {
                return;
            }
            string userID = GetUserID(token);
            //判断玩家是否准备
            if (readyList.Contains(userID))
            {
                return;
            }
            DTOSelectModel sm = null;
            //获取玩家选择数据模型
            if (teamOne.ContainsKey(userID))
            {
                sm = teamOne[userID];
            }
            else if (teamTwo.ContainsKey(userID))
            {
                sm = teamTwo[userID];
            }
            //没有英雄 不让准备
            if (sm.hero == -1)
            {

            }
            else
            {
                //设为已选择 状态 并通知其他人
                sm.isReady = true;
                Brocast(SelectProtocol.READY_BRO, sm);
                readyList.Add(userID);

                if (readyList.Count >= teamOne.Count + teamTwo.Count)
                {
                    //所有人都准备了 开始战斗
                    StartFight();
                }
            }

        }

        private void StartFight()
        {
            if (!string.IsNullOrEmpty(missionID))
            {
                ScheduleUtil.Instance.RemoveMission(missionID);
                missionID = "";
            }
            //通知战斗模块  创建战斗房间
            EventUtil.createFight(teamOne.Values.ToArray(), teamTwo.Values.ToArray());
            Brocast(SelectProtocol.FIGHT_BRO, null);
            //同hi选择房间管理器 销毁当前房间
            EventUtil.destroySelect(roomID);
        }

        /// <summary>
        /// 所有人聊天模式
        /// </summary>
        /// <param name="token"></param>
        /// <param name="value"></param>
        private void AllTalk(UserToken token, string value)
        {
            //判断玩家是否在房间里
            if (!IsEntered(token))
            {
                return;
            }
            UserModel user = GetUser(token);
            Brocast(SelectProtocol.TALK_BRO, user.name + ":" + value); ;

        }


        /// <summary>
        /// 队伍聊天模式
        /// </summary>
        /// <param name="token"></param>
        /// <param name="value"></param>
        private void TeamTalk(UserToken token, string value)
        {
            //判断玩家是否在房间里
            if (!IsEntered(token))
            {
                return;
            }
            UserModel user = GetUser(token);
            if (teamOne.ContainsKey(user.id))
            {
                WriteToUsers(teamOne.Keys.ToArray(), Type, Area, SelectProtocol.SELECT_BRO
                    , _TalkForm(user.name, value));
            }
            else if (teamTwo.ContainsKey(user.id))
            {
                WriteToUsers(teamTwo.Keys.ToArray(), Type, Area, SelectProtocol.SELECT_BRO
                    , _TalkForm(user.name, value));
            }

        }


        string _TalkForm(string name, string value)
        {
            return string.Format("{0}:{1}", name, value);
        }

        private void Select(UserToken token, int selectHeroID)
        {
            //判断玩家是否在房间里
            if (!IsEntered(token))
            {
                return;
            }
            //判断玩家是否拥有此英雄
            UserModel user = GetUser(token);
            if (!user.heroList.Contains(selectHeroID))
            {
                Write(token, SelectProtocol.SELECT_SRES, null);
                return;
            }
            //判断队友是否已经选择这英雄
            DTOSelectModel selectModel = null;
            if (teamOne.ContainsKey(user.id))
            {
                foreach (DTOSelectModel item in teamOne.Values)
                {
                    if (item.hero == selectHeroID)
                    {
                        return;
                    }
                }
                selectModel = teamOne[user.id];

            }
            else if (teamTwo.ContainsKey(user.id))
            {
                foreach (DTOSelectModel item in teamTwo.Values)
                {
                    if (item.hero == selectHeroID)
                    {
                        return;
                    }
                }
                selectModel = teamTwo[user.id];
            }
            //选择成功  通知房间所有人 变更数据
            selectModel.hero = selectHeroID;
            Brocast(SelectProtocol.SELECT_BRO, selectModel);
        }

        private void EnterSelectRoom(UserToken token)
        {
            //判断用户所在房间 并对齐进入状态进行修改
            string userID = GetUserID(token);
            if (teamOne.ContainsKey(userID))
            {
                teamOne[userID].isEnter = true;
            }
            else if (teamTwo.ContainsKey(userID))
            {
                teamTwo[userID].isEnter = true;
            }
            else
            {
                return;
            }
            //判断用户是否已经在房间  不再则计算累加  否则无视
            if (EnterList(token))
            {
                enterCount++;
            }
            //进入成功 发送房间信息给进入的玩家 并通知在房间内的其他玩家有人 进入了房间
            DTOSelectRoom dto = new DTOSelectRoom();
            dto.teamOne = teamOne.Values.ToArray();
            dto.teamTwo = teamTwo.Values.ToArray();
            Write(token, SelectProtocol.ENTER_SRES, dto);
            Brocast(SelectProtocol.ENTER_EXBRO, userID, token);
        }

        private void EnterEvent()
        {
            //判断进入情况 ，如果不是全员进入，则解散房间
            if (enterCount < teamOne.Count + teamTwo.Count)
            {//则说明人员没有全部进来
                Destroy();
            }
            else
            {
                //再次启动定时任务 30内完成选择
                missionID = ScheduleUtil.Instance.Schedule(IsAllSelect, 30000);
            }
        }


        private void IsAllSelect()
        {
            //计时时间是30s 遍历判断 是否所有都选择了英雄
            bool isSelectAll = true;
            foreach (DTOSelectModel item in teamOne.Values)
            {
                if (item.hero == -1)
                {
                    isSelectAll = false;
                    break;
                }
            }
            if (isSelectAll)
            {
                foreach (DTOSelectModel item in teamTwo.Values)
                {
                    if (item.hero == -1)
                    {
                        isSelectAll = false;
                        break;
                    }
                }
            }

            if (isSelectAll)
            {//全部选了英雄  只是有人没有点准备按钮 开始战斗
                StartFight();
            }
            else
            {
                Destroy();
                missionID = "";
            }

        }

        /// <summary>
        /// 解散房间
        /// </summary>
        private void Destroy()
        {
            //通知房间所有人 房间解散了 到回去界面去
            Brocast(SelectProtocol.DESTROY_BRO, null);
            //通知管理器  移除自身
            Clear();
            EventUtil.destroySelect(roomID);
            //当前有定时任务 则进行关闭
            if (missionID != "")
            {
                ScheduleUtil.Instance.RemoveMission(missionID);
            }
        }

        /// <summary>
        /// 清除房间数据
        /// </summary>
        private void Clear()
        {
            list.Clear();
            readyList.Clear();
            teamOne.Clear();
            teamTwo.Clear();
        }
    }
}
