using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class DTOFightBuildModel : AbsFightModel
    {
        public FightUnitType fightUnitType = FightUnitType.BUILD;
        public bool born;//是否重生
        public int bornTime;//重生时间
        public bool initative;//是否可以攻击
        public bool infrared;//红外线（是否反隐）
    }
}
