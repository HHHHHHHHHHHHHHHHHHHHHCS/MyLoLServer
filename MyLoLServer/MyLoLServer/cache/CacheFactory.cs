using MyLoLServer.cache.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.cache
{
    public class CacheFactory
    {
        public readonly static IAccountCache accountCache;
        public readonly static IUserCache userCache;

        static CacheFactory()
        {
            accountCache = new AccountCache();
            userCache = new UserCache();
        }
    }
}
