using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.auto;
using GameProtocol;
using MyLoLServer.biz;
using MyLoLServer.tool;
using GameProtocol.dto;
using MyLoLServer.dao.model;

namespace MyLoLServer.logic.user
{
    public class UserHandler : AbsOnceHandler, InterfaceHandler
    {

        public override byte Type
        {
            get
            {
                return Protocol.TYPE_USER;
            }
        }

        public override int Area
        {
            get
            {
                return UserProtocol.AREA_USER;
            }
        }

        public void ClientClose(UserToken token, string error)
        {
            userBiz.Offline(token);
        }

        /*public void ClientConnect(UserToken token)
        {
            
        }*/

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.command)
            {
                case UserProtocol.CREATE_CREQ:
                    Create(token, message.GetMesssage<string>());
                    break;
                case UserProtocol.INFO_CREQ:
                    Info(token);
                    break;
                case UserProtocol.ONLINE_CREQ:
                    Online(token);
                    break;
            }

        }

        private void Create(UserToken token, string message)
        {
            ExecutorPool.Instance.Execute(delegate ()
            {

                Write(token, UserProtocol.CREATE_SRES
                    , userBiz.Create(token, message));
            });
        }

        private void Info(UserToken token)
        {
            ExecutorPool.Instance.Execute(delegate ()
            {
                UserModel user = userBiz.GetUserByToken(token);
                Write(token, UserProtocol.INFO_SRES
                    , Convert(user));
            });
        }

        private void Online(UserToken token)
        {
            ExecutorPool.Instance.Execute(delegate ()
            {

                Write(token, UserProtocol.ONLINE_SRES
                    , Convert(userBiz.Online(token)));
            });
        }

        private  DTOUser Convert(UserModel user)
        {
            if(user==null)
            {
                return null;
            }
            return new DTOUser()
            {
                id = user.id,
                name = user.name,
                level = user.level,
                nowExp = user.nowExp,
                winCount = user.winCount,
                loseCount = user.loseCount,
                runCount = user.runCount,
                heroList = user.heroList
            };
        }
    }
}
