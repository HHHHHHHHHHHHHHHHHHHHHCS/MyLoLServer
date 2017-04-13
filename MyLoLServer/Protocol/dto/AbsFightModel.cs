using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class AbsFightModel
    {
        public int id;//单位唯一识别码
        public int code;//单位类型ID
        public string name;//名字
        public float hp;//当前血量
        public float maxHp;//最大 血量
        public float mp;//当前魔法值
        public float maxMp;//最大魔法值
        public float attack;//攻击力
        public float armor;//护甲
        public float moveSpeed;//移动速度
        public float atkSpeed;//攻击速度
        public float atkRange;//攻击范围
        public FightUnitType fightUnitType;//战斗单位类型
        public float eyeRange;//视野范围
    }


    /// <summary>
    /// 战斗单位类型
    /// </summary>
    [Serializable]
    public enum FightUnitType
    {
        BUILD,//建筑类型
        NORMAL,//普通单位
        HERO,//英雄
        BOSS//大BOSS类型
    }
}
