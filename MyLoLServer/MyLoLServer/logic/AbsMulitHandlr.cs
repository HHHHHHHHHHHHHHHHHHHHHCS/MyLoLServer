using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.logic
{
    public class AbsMulitHandlr : AbsOnceHandler
    {
        public override byte Type
        {
            get
            {
                return base.Type;
            }

            set
            {
                base.Type = value;
            }
        }


        public override int Area
        {
            get
            {
                return base.Area;
            }

            set
            {
                base.Area = value;
            }
        }


        public List<UserToken> list = new List<UserToken>();


        /// <summary>
        /// 用户进入当前子模块
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool EnterList(UserToken token)
        {
            if (IsEntered(token))
            {
                return false;
            }
            list.Add(token);
            return true;
        }


        /// <summary>
        /// 用户是否在此子模块
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsEntered(UserToken token)
        {
            return list.Contains(token);
        }


        /// <summary>
        /// 用户离开当前子模块
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Leave(UserToken token)
        {
            if (IsEntered(token))
            {
                list.Remove(token);
                return true;
            }
            return false;
        }
        #region 消息群发Brocast


        public void Brocast(int command, object message, UserToken exToken = null)
        {
            Brocast(Area, command, message, exToken);
        }


        public void Brocast(int area, int command, object message, UserToken exToken = null)
        {
            Brocast(Type, area, command, message, exToken);
        }


        public void Brocast(byte type, int area, int command, object message, UserToken exToken = null)
        {
            byte[] value = MessageEncoding.Encode(CreateSocketModel(type, area, command, message));
            value = LengthEncoding.Encode(value);

            byte[] bs = new byte[value.Length];//拷贝一份 以免发的时候出现的修改
            Array.Copy(value, 0, bs, 0, value.Length);

            foreach (UserToken item in list)
            {
                if (item != exToken)
                {
                    item.Write(bs);
                }
            }
        }
        #endregion
    }


}
