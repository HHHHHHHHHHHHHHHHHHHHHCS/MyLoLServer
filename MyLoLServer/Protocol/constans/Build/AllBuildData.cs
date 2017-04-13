using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.constans.Build
{
    public class ZhuJiDi : BuildData.BuildDataModel
    {
        public ZhuJiDi()
        {
            code = 1;
            name = "主基地";
            hp = 5000;
            atk = 0; ;
            arrorm = 50;
            initiative = false;
            infrared = true;
            reborn = false;
            rebornTime = 0;
        }
    }

    public class GaoJiJianTa : BuildData.BuildDataModel
    {
        public GaoJiJianTa()
        {
            code = 2;
            name = "高级箭塔";
            hp = 3000;
            atk = 200;
            arrorm = 50;
            initiative = false;
            infrared = true;
            reborn = true;
            rebornTime = 30;
        }
    }

    public class ZhongJiJianTa : BuildData.BuildDataModel
    {
        public ZhongJiJianTa()
        {
            code = 3;
            name = "中级箭塔";
            hp = 3000;
            atk = 150;
            arrorm = 40;
            initiative = false;
            infrared = true;
            reborn = false;
            rebornTime = 0;
        }
    }

    public class ChuJiJianTa : BuildData.BuildDataModel
    {
        public ChuJiJianTa()
        {
            code = 4;
            name = "初级箭塔";
            hp = 3000;
            atk = 100;
            arrorm = 30;
            initiative = false;
            infrared = true;
            reborn = false;
            rebornTime = 0;
        }
    }
}
