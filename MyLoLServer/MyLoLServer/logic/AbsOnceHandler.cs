using MyLoLServer.biz;
using MyLoLServer.dao.model;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.logic
{
    public class AbsOnceHandler
    {
        public virtual int Area
        {
            get;
            set;
        }

        public virtual byte Type
        {
            get;
            set;
        }

        protected IUserBiz userBiz = BizFactory.userBiz;

        public SocketModel CreateSocketModel(byte type, int area, int command, object message)
        {
            return new SocketModel(type, area, command, message);
        }

        #region 通过连接对象发送
        public void Write(UserToken token, int command)
        {
            Write(token, command, null);
        }

        public void Write(UserToken token, int command, object message)
        {
            Write(token, Area, command, message);
        }

        public void Write(UserToken token, int area, int command, object message)
        {
            Write(token, Type, area, command, message);
        }

        public void Write(UserToken token, byte type, int area, int command, object message)
        {
            byte[] value = MessageEncoding.Encode(CreateSocketModel(type, area, command, message));
            value = LengthEncoding.Encode(value);
            token.Write(value);
        }
        #endregion

        #region 通过ID发送
        public void Write(string id, int command)
        {
            Write(id, command, null);
        }

        public void Write(string id, int command, object message)
        {
            Write(id, Area, command, message);
        }

        public void Write(string id, int area, int command, object message)
        {
            Write(id, Type, area, command, message);
        }

        public void Write(string id, byte type, int area, int command, object message)
        {
            UserToken token = getToken(id);
            if (token == null)
            {
                return;
            }
            Write(token, type, area, command, message);
        }

        public void WriteToUsers(string[] userIDs, byte type, int area, int command, object message)
        {
            byte[] value = MessageEncoding.Encode(CreateSocketModel(type, area, command, message));
            value = LengthEncoding.Encode(value);

            byte[] bs = new byte[value.Length];//拷贝一份 以免发的时候出现的修改
            Array.Copy(value, 0, bs, 0, value.Length);

            foreach (string item in userIDs)
            {
                UserToken token = userBiz.GetToken(item);
                if(token!=null)
                {
                    token.Write(bs);
                }
            }
        }
        #endregion

        /// <summary>
        /// 通过连接对象获取用户
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserModel GetUser(UserToken token)
        {
            return userBiz.GetUserByToken(token);
        }

        /// <summary>
        /// 通过连接对象获取用户
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public UserModel GetUser(string userID)
        {
            return userBiz.GetUserByUserID(userID);
        }

        /// <summary>
        /// 通过连接对象 获取用户ID
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string GetUserID(UserToken token)
        {
            UserModel user = GetUser(token);
            if (user == null)
            {
                return "";

            }
            return user.id;
        }
        /// <summary>
        /// 通过用户ID获取连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserToken getToken(string userID)
        {
            return userBiz.GetToken(userID);
        }
    }
}
