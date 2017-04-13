using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.tool
{
    public class TimeTaskModel
    {
        /// <summary>
        /// 任务逻辑
        /// </summary>
        private TimeEvent execut;

        /// <summary>
        /// 任务执行的时间
        /// </summary>
        public long time;

        /// <summary>
        /// 任务ID
        /// </summary>
        public string id;

        public TimeTaskModel(string _id, TimeEvent _execut, long _time)
        {
            id = _id;
            execut = _execut;
            time = _time;
        }

        public void Run()
        {
            execut();
        }
    }
}
