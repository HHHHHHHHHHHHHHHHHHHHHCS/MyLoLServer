using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLoLServer.tool
{
    public class ConcurrentStringID
    {
        Mutex tex = new Mutex();
        int index = 0;
        string lastTime = "";

        public ConcurrentStringID()
        {

        }

        public string GetNewStringID(int length = 10)
        {
            lock (this)
            {
                tex.WaitOne();
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (lastTime != nowTime)
                {
                    index = 0;
                    lastTime = nowTime;
                }

                string newStringID = string.Format("{0}{1}"
                    , DateTime.Now.ToString("yyyyMMddHHmmss")
                    , index.ToString().PadLeft(length, '0'));
                index++;

                tex.ReleaseMutex();
                return newStringID;
            }
        }

        public string GetNewGUID()
        {
            lock (this)
            {
                tex.WaitOne();
                string newStringID = string.Format("{0}-{1}"
                    , DateTime.Now.ToString("yyyyMMddHHmmss")
                    , Guid.NewGuid().ToString());
                tex.ReleaseMutex();
                return newStringID;
            }
        }
    }
}
