using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class DTOFightPlayerModel:AbsFightModel
    {
        public string userID;
        public int level;//等级
        public int exp;//当前经验值
        public int freeSkillPoint;//剩余技能点
        public int money;//钱
        public int[] equs;//玩家装备列表
        public DTOFightSkill[] skillList;//英雄技能表
    }
}
