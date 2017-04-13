using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    public class SelectProtocol
    {
        public const int AREA_SELECT = 0;

        public const int ENTER_CREQ = 0;//客户端请求 进入
        public const int ENTER_SRES = 1;//服务器 返回进入请求
        public const int ENTER_EXBRO = 2;//服务器 返回额外的人 进入请求
        public const int SELECT_CREQ = 3;//客户端选择英雄请求
        public const int SELECT_SRES = 4;//服务器返回选择人请求结果
        public const int SELECT_BRO = 5;//服务器返回选人 广播
        public const int TALK_CREQ = 6;//客户端交流请求
        public const int TALK_BRO = 7;//服务器交流返回
        public const int READY_CREQ = 8;//客户端准备
        public const int READY_BRO = 9;//服务器返回准备的广播
        public const int DESTROY_BRO = 10;//服务器摧毁房间的广播
        public const int FIGHT_BRO = 11;//服务器开始战斗的广播
    }
}
