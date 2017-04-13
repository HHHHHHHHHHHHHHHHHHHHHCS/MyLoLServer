using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.cache
{
    public interface IAccountCache
    {
        /// <summary>
        /// 帐号是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool HasAccount(string account);

        /// <summary>
        /// 帐号密码是否匹配
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool MatchAccount(string account, string password);


        /// <summary>
        /// 帐号是否在线
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool IsOnline(string account);


        /// <summary>
        /// 当前连接对象所对应的ID
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        string GetID(UserToken token);

        /// <summary>
        /// 帐号上线
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        void Online(UserToken token,string account);

        /// <summary>
        /// 用户下线
        /// </summary>
        /// <param name="token"></param>
        void Offline(UserToken token);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        void Add(string account, string password);
    }
}
