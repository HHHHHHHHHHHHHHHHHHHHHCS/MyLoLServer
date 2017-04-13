using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    public class LoginProtocol
    {
        public const int AREA_LOGIN = 0;//暂时没有用

        public const int LOGIN_CREQ = 0;//登录端申请登录
        public const int LOGIN_SRES = 1;//服务器反馈给客户端 登录结果

        public const int REG_CREQ = 2;//登录申请注册
        public const int REG_SRES = 3;//服务器反馈给客户端 注册结果
    }
}
