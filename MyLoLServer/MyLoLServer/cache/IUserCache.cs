using MyLoLServer.dao.model;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.cache
{
    public interface IUserCache
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        bool Create(UserToken token, string name, string accountID);

        /// <summary>
        /// 是否拥有角色
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool Has(UserToken token);

        /// <summary>
        /// 通过对应帐号ID判断是否拥有角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool HasByAccountID(string id);

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetUser(string id);

        /// <summary>
        /// 根据连接获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel GetUser(UserToken token);


        /// <summary>
        /// 通过ID获取连接对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserToken GetToken(string id);

        /// <summary>
        /// 用户离线
        /// </summary>
        /// <param name="token"></param>
        void Offline(UserToken token);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel Online(UserToken token, string id);


        /// <summary>
        /// 通过帐号ID获取角色
        /// </summary>
        /// <param name="accID"></param>
        /// <returns></returns>
        UserModel GetUserByAccountID(string accID);


        /// <summary>
        /// 判断角色是否已经在线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool isOnline(string id);
    }
}
