using MyLoLServer.logic;
using MyLoLServer.logic.login;
using NetFrame;
using NetFrame.auto;
using System;
using GameProtocol;
using MyLoLServer.logic.user;
using MyLoLServer.logic.match;
using MyLoLServer.logic.select;
using MyLoLServer.logic.fight;

namespace MyLoLServer
{
    class HandlerCenter : AbsHandlerCenter
    {

        InterfaceHandler login;
        InterfaceHandler user;
        InterfaceHandler match;
        InterfaceHandler select;
        InterfaceHandler fight;

        public HandlerCenter()
        {
            login = new LoginHandler();
            user = new UserHandler();
            match = new MatchHandler();
            select = new SelectHandler();
            fight = new FightHandler();
        }

        public override void ClientClose(UserToken token, string error)
        {
            Console.WriteLine("有客户端断开连接！");
            //User的连接诶关闭方法一定要房子啊逻辑处理单元后面
            //其他逻辑单元需要通过user版定的数据来进行内存清理
            //如果先清除了绑定关系 其他模块无法获取角色数据会导致无法清理
            fight.ClientClose(token, error);
            select.ClientClose(token, error);
            match.ClientClose(token, error);
            login.ClientClose(token, error);
            user.ClientClose(token, error);
        }

        public override void ClientConnect(UserToken token)
        {
            Console.WriteLine("有客户端开始连接！");
        }

        public override void MessageReceive(UserToken token, object message)
        {
            SocketModel model = message as SocketModel;
            switch (model.type)
            {
                case Protocol.TYPE_LOGIN:
                    login.MessageReceive(token, model);
                    break;
                case Protocol.TYPE_USER:
                    user.MessageReceive(token, model);
                    break;
                case Protocol.TYPE_MATCH:
                    match.MessageReceive(token, model);
                    break;
                case Protocol.TYPE_SELECT:
                    select.MessageReceive(token, model);
                    break;
                case Protocol.TYPE_FIGHT:
                    fight.MessageReceive(token, model);
                    break;
                case Protocol.TYPE_HEARTBEAT:
                    //心跳包
                    break;
                default:
                    //未知模块 可能是客户端作弊了 无视
                    break;
            }
        }
    }
}
