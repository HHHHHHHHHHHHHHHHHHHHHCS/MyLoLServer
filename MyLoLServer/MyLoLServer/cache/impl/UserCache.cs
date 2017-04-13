using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLoLServer.dao.model;
using NetFrame;

namespace MyLoLServer.cache.impl
{
    public class UserCache : IUserCache
    {
        /// <summary>
        /// 用户ID和模型的映射表
        /// </summary>
        Dictionary<string, UserModel> idToModel = new Dictionary<string, UserModel>();

        /// <summary>
        /// 帐号ID和角色ID之间的绑定
        /// </summary>
        Dictionary<string, string> accToUserID = new Dictionary<string, string>();

        /// <summary>
        /// 双向映射表 id和用户连接
        /// </summary>
        Dictionary<string, UserToken> idToToken = new Dictionary<string, UserToken>();
        /// <summary>
        /// 双向映射表 用户连接和id
        /// </summary>
        Dictionary<UserToken, string> tokenToID = new Dictionary<UserToken, string>();


        public bool Create(UserToken token, string name, string accountID)
        {
            UserModel user = new UserModel(Guid.NewGuid().ToString(), name, accountID);
            //添加初始英雄
            user.heroList.AddRange(StartHeroList());
            //创建成功 进行帐号ID和用户ID的绑定
            accToUserID.Add(accountID, user.id);
            //创建成功 进行帐号ID和用户模型的绑定
            idToModel.Add(user.id, user);

            return true;
        }

        public UserModel GetUser(UserToken token)
        {
            if (!Has(token))
            {
                return null;
            }
            return idToModel[tokenToID[token]];
        }

        public UserModel GetUser(string id)
        {
            return idToModel[id];
        }

        public UserModel GetUserByAccountID(string accID)
        {
            if (!accToUserID.ContainsKey(accID))
            {
                return null;
            }
            return idToModel[accToUserID[accID]];
        }

        public UserToken GetToken(string id)
        {
            return idToToken[id];
        }

        public bool Has(UserToken token)
        {
            return tokenToID.ContainsKey(token);
        }

        public bool HasByAccountID(string id)
        {
            return idToToken.ContainsKey(id);
        }

        public void Offline(UserToken token)
        {
            if (tokenToID.ContainsKey(token))
            {
                if (idToToken.ContainsKey(tokenToID[token]))
                {
                    idToToken.Remove(tokenToID[token]);
                }
                tokenToID.Remove(token);
            }
        }

        public UserModel Online(UserToken token, string id)
        {
            idToToken.Add(id, token);
            tokenToID.Add(token, id);
            return idToModel[id];
        }

        public bool isOnline(string id)
        {
            return idToToken.ContainsKey(id);
        }


        private List<int> StartHeroList()
        {
            List<int> newList = new List<int>();
            for(int i=1;i<=12;i++)
            {
                newList.Add(i);
            }
            return newList;
        }
    }
}
