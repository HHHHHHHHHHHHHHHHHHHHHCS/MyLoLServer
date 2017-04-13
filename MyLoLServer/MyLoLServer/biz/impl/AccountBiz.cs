using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using MyLoLServer.cache;

namespace MyLoLServer.biz.impl
{
    public class AccountBiz : IAccountBiz
    {
        IAccountCache accountCache = CacheFactory.accountCache;

        public void Close(UserToken token)
        {
            accountCache.Offline(token);
        }

        public int CreateAccount(UserToken token, string account, string password)
        {
            if (accountCache.HasAccount(account))
            {
                return -1;
            }
            accountCache.Add(account, password);
            return 0;
        }

        public string GetAccountID(UserToken token)
        {
            return accountCache.GetID(token);
        }

        public int LoginAccount(UserToken token, string account, string password)
        {
            //帐号密码为空 输入不合法
            if (account == null || password == null)
            {
                return -4;
            }
            //判断帐号是否存在 不存在则无法登录
            if (!accountCache.HasAccount(account))
            {
                return -1;
            }
            //判断此帐号当前是否在线
            if (accountCache.IsOnline(account))
            {
                return -2;
            }
            //判断帐号密码是否匹配
            if (!accountCache.MatchAccount(account, password))
            {
                return -3;
            }
            //验证都通过 说明可以登录 调用上线并返回成功
            accountCache.Online(token, account);
            return 0;
        }
    }
}
