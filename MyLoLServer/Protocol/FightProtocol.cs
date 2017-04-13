using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    public class FightProtocol
    {
        public const int AREA_FIGHT = 0;//区域码

        public const int ENTER_CREQ = 0;//加载完成，进入游戏，需要数据了
        public const int START_BRO = 1;//全部人加载完成，进入游戏

        public const int MOVE_CREQ = 2;//移动
        public const int MOVE_BRO = 3;//移动广播

        public const int SKILL_UP_CREQ = 4;//申请升级技能
        public const int SKILL_UP_SRES = 5;//返回技能升级结果

        public const int ATTACK_CREQ = 6;//申请攻击
        public const int ATTACL_BRO = 7;//群发播放攻击

        public const int DAMAGE_CREA = 8;//伤害事件申请
        public const int DAMAGE_BRO = 9;//群发伤害

        public const int SKILL_CREQ = 10;//申请释放技能
        public const int SKILL_BRO = 11;//群发播放技能

    }
}
