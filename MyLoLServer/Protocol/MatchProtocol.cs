using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    public class MatchProtocol
    {
        public const int AREA_MATCH = 0;//匹配的区域码

        public const int ENTER_CREQ = 0;//申请进入匹配
        public const int ENTER_SRES = 1;//返回申请结果
        public const int LEAVE_CREQ = 2;//申请离开匹配
        public const int LEAVE_SRES = 3;//返回离开结果

        public const int ENTER_SELECT_BRO = 4;//匹配完毕，通知进入 选择界面广播


        public const int TAG_TEAMONE = 10001;//队伍一的标志
        public const int TAG_TEAMTWO = 10002;//队伍二的标志
    }
}
