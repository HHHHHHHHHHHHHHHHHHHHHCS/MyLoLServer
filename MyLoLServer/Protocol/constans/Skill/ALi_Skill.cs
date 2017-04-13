using GameProtocol.dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.constans.Skill
{
    public class ALi_Skill
    {
        public class QiZhaBaoZhu : SkillData.SkillDataModel
        {
            public QiZhaBaoZhu()
            {
                code = 1;
                name = "欺诈宝珠";
                info = "消耗 65/70/75/80/85法力 \n冷却7/7/7/7/7 \n射程 880 \n效果 阿狸放出宝珠，造成40/65/90/115/140(+0.35)魔法伤害，随后将其收回，造成40/65/90/115/140(+0.35)真实伤害。";
                type = SkillType.POSITION;
                target = SkillTarget.FRIEND_NO_BUILD;
                levels = new SkillData.SkillLevelData[] {
                    SkillData.NewSkillLevelData(1,0,0,0),
                    SkillData.NewSkillLevelData(3,7,65,880),
                    SkillData.NewSkillLevelData(5,7,70,880),
                    SkillData.NewSkillLevelData(7,7,75,880),
                    SkillData.NewSkillLevelData(9,7,80,880),
                };
            }
        }
        public class YaoYiHuHuo : SkillData.SkillDataModel
        {
            public YaoYiHuHuo()
            {
                code = 2;
                name = "欺诈宝珠";
                info = "消耗 50/50/50/50/50法力 \n冷却 9/8/7/6/5 \n射程 550 \n效果 阿狸放出三团狐火，狐火会锁定附近的三名敌人（英雄优先）进行攻击，造成40/65/90/115/140(+0.4)魔法伤害。";
                type = SkillType.SELF;
                target = SkillTarget.Enemy_Hero;
                levels = new SkillData.SkillLevelData[] {
                    SkillData.NewSkillLevelData(1, 0, 0, 0),
                        SkillData.NewSkillLevelData(3, 7, 65, 880),
                        SkillData.NewSkillLevelData(5, 7, 70, 880),
                        SkillData.NewSkillLevelData(7, 7, 75, 880),
                        SkillData.NewSkillLevelData(9, 7, 80, 880),
                    };
            }
        }

        public class MeiHuoYaoShu : SkillData.SkillDataModel
        {
            public MeiHuoYaoShu()
            {
                code = 3;
                name = "魅惑妖术";
                info = "消耗 85/85/85/85/85法力 \n冷却 12/12/12/12/12 \n射程 975 \n效果 阿狸献出红唇热吻，对命中的第一个敌人造成60/90/120/150/200(+0.5)魔法伤害并将目标魅惑，让目标意乱情迷地走向阿狸。魅惑效果持续1/1.25/1.5/1.75/2秒。";
                type = SkillType.POSITION;
                target = SkillTarget.Enemy_Hero;
                levels = new SkillData.SkillLevelData[] {
                    SkillData.NewSkillLevelData(1, 0, 0, 0),
                        SkillData.NewSkillLevelData(3, 7, 65, 880),
                        SkillData.NewSkillLevelData(5, 7, 70, 880),
                        SkillData.NewSkillLevelData(7, 7, 75, 880),
                        SkillData.NewSkillLevelData(9, 7, 80, 880),
                    };
            }
        }


        public class LinHunTuXi : SkillData.SkillDataModel
        {
            public LinHunTuXi()
            {
                code = 4;
                name = "灵魂突袭";
                info = "消耗 100/100/100法力 \n冷却 110/95/80 \n射程 450 \n效果 阿狸像妖魅一般向前冲锋，并向周围的3名敌人（英雄优先）发射元气弹，造成70/110/150(+0.3)魔法伤害。灵魄突袭可以在进入冷却阶段前的10秒内被施放最多3次。";
                type = SkillType.POSITION;
                target = SkillTarget.Enemy_Hero;
                levels = new SkillData.SkillLevelData[] {
                    SkillData.NewSkillLevelData(1, 0, 0, 0),
                        SkillData.NewSkillLevelData(3, 7, 65, 880),
                        SkillData.NewSkillLevelData(5, 7, 70, 880),
                        SkillData.NewSkillLevelData(7, 7, 75, 880),
                        SkillData.NewSkillLevelData(9, 7, 80, 880),
                    };
            }

        }
    }

}
