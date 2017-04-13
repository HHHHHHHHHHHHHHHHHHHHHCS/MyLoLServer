using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLoLServer.tool
{
    public delegate void ExecutorDelegate();

    /// <summary>
    /// 单线程处理对象 将所有事物处理调用 通过此处调用
    /// </summary>
    public class ExecutorPool
    {
        private static ExecutorPool instance;

        /// <summary>
        /// 单例对象
        /// </summary>
        public static ExecutorPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExecutorPool();
                }
                return instance;
            }
        }

        /// <summary>
        /// 线程同步锁
        /// </summary>
        Mutex tex = new Mutex();


        /// <summary>
        /// 单线程处理逻辑
        /// </summary>
        /// <param name="ed"></param>
        public void Execute(ExecutorDelegate ed)
        {
            lock(this)
            {
                tex.WaitOne();
                ed();
                tex.ReleaseMutex();
            }
        }
    }
}
