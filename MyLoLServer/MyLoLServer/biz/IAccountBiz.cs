using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.biz
{
    public interface IAccountBiz
    {
        /// <summary>
        /// 帐号创建
        /// </summary>
        /// <param name="token">客户端用户</param>
        /// <param name="account">注册的帐号</param>
        /// <param name="password">注册的密码</param>
        /// <returns>返回创建结果 0成功 -1帐号已经存在 -2帐号不合法 -3密码不合法</returns>
        int CreateAccount(UserToken token, string account, string password);


        /// <summary>
        /// 帐号登录
        /// </summary>
        /// <param name="token">客户端用户</param>
        /// <param name="account">帐号</param>
        /// <param name="password">密码</param>
        /// <returns>登录结果 0成功  -1帐号不存在 -2帐号已经登录 -3帐号密码错误 -4输入不合法</returns>
        int LoginAccount(UserToken token, string account, string password);


        /// <summary>
        /// 客户端断开连接(下线)
        /// </summary>
        /// <param name="token"></param>
        void Close(UserToken token);

        /// <summary>
        /// 获取帐号ID
        /// </summary>
        /// <param name="token"></param>
        /// <returns>返回用户登录的帐号ID</returns>
        string GetAccountID(UserToken token);
    }

}
