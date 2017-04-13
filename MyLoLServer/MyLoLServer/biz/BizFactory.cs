using MyLoLServer.biz.impl;

namespace MyLoLServer.biz
{
    public class BizFactory
    {
        public static readonly IAccountBiz accountBiz;
        public static readonly IUserBiz userBiz;

        static BizFactory()
        {
            accountBiz = new AccountBiz();
            userBiz = new UserBiz();
        }
    }
}
