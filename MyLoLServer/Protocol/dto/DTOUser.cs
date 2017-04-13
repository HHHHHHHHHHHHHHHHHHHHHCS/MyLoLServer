using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class DTOUser
    {
        public string id;//玩家ID 唯一组件
        public string accountID;//角色所属的帐号ID
        public string name;//玩家昵称
        public int level;//玩家等级
        public int nowExp;//玩家当前经验
        public int winCount;//胜利场次
        public int loseCount;//失败场次
        public int runCount;//逃跑场次
        public List<int> heroList;//拥有的英雄列表

        public DTOUser()
        {

        }


    }
}
