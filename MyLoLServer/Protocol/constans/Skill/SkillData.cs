using GameProtocol.dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.constans.Skill
{
    public class SkillData
    {
        private static readonly Dictionary<int, SkillDataModel> skillMap = new Dictionary<int, SkillDataModel>();


        public partial class SkillDataModel
        {
            public int code;
            public SkillLevelData[] levels;//技能升级等级
            public string name;
            public string info;
            public SkillTarget target;//技能目标类型
            public SkillType type;
        }


        [Serializable]
        public partial class SkillLevelData
        {
            public int level;//学习等级
            public int time;//冷却时间
            public int mp;//耗蓝
            public float range;//攻击范围

            public SkillLevelData() { }
            public SkillLevelData(int level, int time, int mp, float range)
            {
                this.level = level;
                this.time = time;
                this.mp = mp;
                this.range = range;
            }
        }



        static SkillData()
        {
            //阿狸
            SkillDataModel ALi_QiZhaBaoZhu = new ALi_Skill.QiZhaBaoZhu();
            Create(ALi_QiZhaBaoZhu);
            SkillDataModel ALi_YaoYiHuHuo = new ALi_Skill.YaoYiHuHuo();
            Create(ALi_YaoYiHuHuo);
            SkillDataModel ALi_MeiHuoYaoShu = new ALi_Skill.MeiHuoYaoShu();
            Create(ALi_MeiHuoYaoShu);
            SkillDataModel ALi_LinHunTuXi = new ALi_Skill.LinHunTuXi();
            Create(ALi_LinHunTuXi);
        }

        public static SkillDataModel GetSkillDataByID(int id)
        {
            return skillMap[id];
        }

        public static SkillLevelData NewSkillLevelData(int level, int time, int mp, float range)
        {
            SkillLevelData data = new SkillLevelData(level, time, mp, range);
            return data;
        }

        static void Create(SkillDataModel skillData)
        {
            if (!skillMap.ContainsKey(skillData.code))
            {
                skillMap.Add(skillData.code, skillData);
            }
        }

    }



}
