using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLoLServer.tool
{
    public class ConcurrentIntegerID
    {
        int value=0;
        Mutex tex = new Mutex();

        public ConcurrentIntegerID() { }
        public ConcurrentIntegerID(int value) { this.value = value; }
        /// <summary>
        /// 自增并返回值
        /// </summary>
        /// <returns></returns>
        public int GetAndAdd()
        {
            lock (this)
            {
                tex.WaitOne();
                if(value==int.MaxValue)
                {
                    value = 0;
                }
                value++;
                tex.ReleaseMutex();
                return value;
            }
        }
        /// <summary>
        /// 自减并返回值
        /// </summary>
        /// <returns></returns>
        public int GetAndReduce()
        {
            lock (this)
            {
                tex.WaitOne();
                value--;
                tex.ReleaseMutex();
                return value;
            }
        }
        /// <summary>
        /// 重置value为0
        /// </summary>
        public void Reset()
        {
            lock (this)
            {
                tex.WaitOne();
                value = 0;
                tex.ReleaseMutex();

            }
        }

        public int Get()
        {
            return value;
        }
    }

}
