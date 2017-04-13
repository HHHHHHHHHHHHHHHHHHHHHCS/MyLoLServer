using MyLoLServer.dao.model;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.biz
{
    public interface IUserBiz
    {
        /// <summary>
        /// 创建召唤师
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Create(UserToken token, string name);

        /// <summary>
        /// 获取连接对应的用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel GetUserByToken(UserToken token);

        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        UserModel GetUserByUserID(string userID);

        /// <summary>
        /// 通过帐号ID来获取用户  仅在初始登录验证角色时有效
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        UserModel GetUserByAccountID(string accountID);

        /// <summary>
        /// 用户上线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel Online(UserToken token);

        /// <summary>
        /// 用户下线 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        void Offline(UserToken token);

        /// <summary>
        /// 通过ID获取连接对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserToken GetToken(string id);
    }
}
