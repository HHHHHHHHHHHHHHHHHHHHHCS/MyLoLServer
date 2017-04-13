using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.dao.model;
using NetFrame;
using MyLoLServer.cache;

namespace MyLoLServer.biz.impl
{
    /// <summary>
    /// 用户事物处理
    /// </summary>
    public class UserBiz : IUserBiz
    {
        IAccountBiz accBiz = BizFactory.accountBiz;
        IUserCache userCache = CacheFactory.userCache;

        public bool Create(UserToken token, string name)
        {
            //帐号是否登录 获取帐号ID
            string accountID = accBiz.GetAccountID(token);
            if (string.IsNullOrEmpty(accountID))
            {
                return false;
            }
            //判断当前帐号是否已经拥有角色
            if (userCache.HasByAccountID(accountID))
            {
                return false;
            }

            userCache.Create(token, name,accountID);
            return true;
        }

        public UserModel GetUserByUserID(string userID)
        {
            return userCache.GetUser(userID);
        }

        public UserModel GetUserByToken(UserToken token)
        {
            //帐号是否登录获取帐号ID
            string accountID = accBiz.GetAccountID(token);
            if(string.IsNullOrEmpty(accountID))
            {
                return null;
            }
            return userCache.GetUserByAccountID(accountID);
        }

        public UserModel GetUserByAccountID(string accountID)
        {
            return userCache.GetUserByAccountID(accountID);
        }

        public UserToken GetToken(string id)
        {
            return userCache.GetToken(id);
        }

        public void Offline(UserToken token)
        {
            userCache.Offline(token);
        }

        public UserModel Online(UserToken token)
        {
            //帐号是否登录 获取帐号ID
            string accountID = accBiz.GetAccountID(token);
            if (string.IsNullOrEmpty(accountID))
            {
                return null;
            }
            UserModel user=userCache.GetUserByAccountID(accountID);
            if (userCache.isOnline(user.id))
            {
                return null;
            }
            userCache.Online(token, user.id);
            return user;
        }


    }
}
