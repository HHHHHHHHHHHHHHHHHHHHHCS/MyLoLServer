using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.constans.Build
{
    public class BuildData
    {
        private static readonly Dictionary<int, BuildDataModel> buildMap = new Dictionary<int, BuildDataModel>();

        /// <summary>
        /// 局部类
        /// </summary>
        public partial class BuildDataModel
        {
            public int code;//箭塔编码
            public string name;//名字
            public int hp;//箭塔血量
            public int atk;//箭塔攻击力
            public int arrorm;//箭塔防御
            public bool initiative;//是否是攻击建筑
            public bool infrared;//红外线 反隐
            public bool reborn;//是否可以复活
            public int rebornTime;//复活时间，单位秒
        }

        public static BuildDataModel GetBuildDataByID(int id)
        {
            if(!buildMap.ContainsKey(id))
            {
                return null;
            }
            return buildMap[id];
        }

        static void Create(BuildDataModel buildData)
        {
            if (!buildMap.ContainsKey(buildData.code))
            {
                buildMap.Add(buildData.code, buildData);
            }
        }

        static BuildData()
        {
            BuildDataModel zhuJiDi = new ZhuJiDi();
            Create(zhuJiDi);

            BuildDataModel gaoJiJianTa = new GaoJiJianTa();
            Create(gaoJiJianTa);


            BuildDataModel zhongJiJianTa = new ZhongJiJianTa();
            Create(zhongJiJianTa);

            BuildDataModel chuJiJianTa = new ChuJiJianTa();
            Create(chuJiJianTa);
        }
    }
}
