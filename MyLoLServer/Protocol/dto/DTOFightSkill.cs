using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public  class DTOFightSkill
    {
        public int code;
        public int level;
        public int studyNextLevel;
        public int cdTime;//冷却时间 -MS
        public string name;//技能名称
        public string skillDes;//技能描述
        public SkillType skillType;//技能类型
        public SkillTarget skillTarget;//技能释放单位
        public float range;//技能释放距离
    }

    /// <summary>
    /// 能够造成效果的单位类型
    /// </summary>
    [Serializable]
    public enum SkillTarget
    {
        SELF,//自身为中心释放
        FRIEND_HERO,//友方英雄
        FRIEND_NO_BUILD,//有方非建筑单位
        FRIEND_ALL,//有方全部
        FRIND_NO_ME,//不包括自己的有方全部单位
        FRIND_NO_ME_BUILD,//不包括自己和建筑的有方全部但我i额
        FRIND_HERO_NO_ME_BUILD,//不包括自己和建筑物的友方英雄
        Enemy_Hero,//敌方英雄
        Enemy_NO_BUILD,//敌方非建筑
        Enemy_SOLDIER_NETURAL,//敌方小兵和中立单位
        NO_FRIEND_ALL,//非有方单位
    }


    /// <summary>
    /// 技能释放方式类型
    /// </summary>
    [Serializable]
    public enum SkillType
    {
        SELF,//以自身为中心进行释放
        TARGET,//以目标为中心进行释放
        POSITION,//以鼠标点击为中心进行释放
        TARGET_POSITION,//可以指定和位置
    }

}
