using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol.dto
{
    [Serializable]
    public class DTOSelectModel
    {
        public string userID;//用户ID
        public string name;//用户名称
        public int hero;//所选英雄
        public bool isEnter;//是否进入
        public bool isReady;//是否已准备
    }
}
