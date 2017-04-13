using GameProtocol;
using GameProtocol.dto;
using MyLoLServer.biz;
using MyLoLServer.tool;
using NetFrame;
using NetFrame.auto;
using System;

namespace MyLoLServer.logic.login
{
    public class LoginHandler : AbsOnceHandler, InterfaceHandler
    {

        public override byte Type
        {
            get
            {
                return Protocol.TYPE_LOGIN;
            }
        }

        public override int Area
        {
            get
            {
                return LoginProtocol.AREA_LOGIN;
            }
        }

        IAccountBiz accountBiz = BizFactory.accountBiz;

        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.command)
            {
                case LoginProtocol.LOGIN_CREQ:
                    {
                        Login(token, message.GetMesssage<DTOAccountInfo>());
                        break;
                    }
                case LoginProtocol.REG_CREQ:
                    {
                        Reg(token, message.GetMesssage<DTOAccountInfo>());
                        break;
                    }
            }
        }

        public void Login(UserToken token, DTOAccountInfo value)
        {
            ExecutorPool.Instance.Execute(delegate ()
            {
                int result = accountBiz.LoginAccount(token, value.Account, value.Password);
                Write(token, LoginProtocol.LOGIN_SRES, result);
            });

        }

        public void Reg(UserToken token, DTOAccountInfo value)
        {
            ExecutorPool.Instance.Execute(delegate ()
            {
                int result = accountBiz.CreateAccount(token, value.Account, value.Password);
                Write(token, LoginProtocol.REG_SRES, result);
            });
        }

        public void ClientClose(UserToken token, string error)
        {
            ExecutorPool.Instance.Execute(delegate ()
            {
                accountBiz.Close(token);
            });
        }

        /*public void ClientConnect(UserToken token)
        {
        }*/
    }


}
