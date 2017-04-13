using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.constans.Hero
{
    public class HeroData
    {
        private static readonly Dictionary<int, HeroDataModel> heroDataMap = new Dictionary<int, HeroDataModel>();


        /// <summary>
        /// 局部类
        /// </summary>
        public partial class HeroDataModel
        {
            public int code;//策划定义的唯一编号
            public string name;//英雄的名称
            public float atkBase;//基础攻击力
            public float armorBase;//基础防御力
            public float hpBase;//初始血量
            public float mpBase;//初始魔法值
            public float atkGrow;//攻击成长
            public float armorGrow;//防御成长
            public float hpGrow;//生命成长
            public float mpGrow;//防御成长
            public float moveSpeedBase;//基础移动速度
            public float atkSpeedBase;//基础攻击速度
            public float atkSpeedGrow;//攻击速度成长
            public float atkRangeBase;//基础攻击距离
            public float eyeRangeBase;//视野范围
            public int[] skillsBase;//拥有的技能
        }


        private static void Create(HeroDataModel heroDataModel)
        {
            if(!heroDataMap.ContainsKey(heroDataModel.code))
            {
                heroDataMap.Add(heroDataModel.code, heroDataModel);
            }

        }

        public static HeroDataModel FindHeroDataByID(int id)
        {
            if(!heroDataMap.ContainsKey(id))
            {
                return null;
            }
            return heroDataMap[id];
        }

        /// <summary>
        /// 静态构造  比  实例化构造更早 ，当第一次引用该类的时候就会启用
        /// </summary>
        static HeroData()
        {
            HeroDataModel ALi = new ALi();
            Create(ALi);

            HeroDataModel AMuMu = new AMuMu();
            Create(AMuMu);

            HeroDataModel AiXi = new AiXi();
            Create(AiXi);

            HeroDataModel MangSeng = new MangSeng();
            Create(MangSeng);

        }



    }
}
