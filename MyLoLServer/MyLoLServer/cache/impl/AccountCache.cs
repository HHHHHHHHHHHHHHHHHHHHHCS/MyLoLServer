using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using MyLoLServer.dao.model;

namespace MyLoLServer.cache.impl
{
    public class AccountCache : IAccountCache
    {
        /// <summary>
        /// 玩家连接对象与帐号的映射绑定
        /// </summary>
        Dictionary<UserToken, string> onlineAccMap = new Dictionary<UserToken, string>();

        /// <summary>
        /// 帐号自身具体属性的映射绑定
        /// </summary>
        Dictionary<string, AccountModel> accMap = new Dictionary<string, AccountModel>();


        public void Add(string account, string password)
        {
            //创建帐号尸体并进行绑定
            AccountModel model = new AccountModel(Guid.NewGuid().ToString()
                ,account,password);
            accMap.Add(account,model);
        }

        public string GetID(UserToken token)
        {
            //判断在线字典中是否有此连接的记录 没有说明此连接没有登录 无法获取到帐号
            if(!onlineAccMap.ContainsKey(token))
            {
                return "";
            }
            if(!accMap.ContainsKey(onlineAccMap[token]))
            {
                return "";//作弊登录
            }
            return accMap[onlineAccMap[token]].Id;
        }

        public bool HasAccount(string account)
        {
            return accMap.ContainsKey(account);
        }

        public bool IsOnline(string account)
        {
            //判断当前在线字典中是否有此帐号  没有则说明不在线
            return onlineAccMap.ContainsValue(account);
        }

        public bool MatchAccount(string account, string password)
        {
            //判断帐号是否存在 不存在就谈不上匹配
            if(!HasAccount(account))
            {
                return false;
            }
            return accMap[account].Password.Equals(password);
        }

        public void Offline(UserToken token)
        {
            if(onlineAccMap.ContainsKey(token))
            {
                onlineAccMap.Remove(token);
            }
        }

        public void Online(UserToken token, string account)
        {
            //添加映射
            onlineAccMap.Add(token,account);
        }
    }
}
