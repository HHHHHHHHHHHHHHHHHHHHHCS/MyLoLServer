using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MyLoLServer.tool
{
    public delegate void TimeEvent();
    public class ScheduleUtil
    {
        private static ScheduleUtil util;

        public static ScheduleUtil Instance
        {
            get
            {
                if (util == null)
                {
                    util = new ScheduleUtil();
                }
                return util;
            }
        }

        Timer timer;

        private ConcurrentStringID IDFactory = new ConcurrentStringID();

        //等待执行的任务列表
        private ConcurrentDictionary<string, TimeTaskModel> mission = new ConcurrentDictionary<string, TimeTaskModel>();
        //等待移除的任务列表
        private List<string> removeList = new List<string>();

        private ScheduleUtil()
        {
            timer = new Timer(200);//200毫秒执行一次
            //timer.Elapsed 时间到了处理方法
            timer.Elapsed += CallBack;
            //timer.AutoReset = true;//设置一直执行 默认是true
            timer.Start();
        }

        void CallBack(object sender, ElapsedEventArgs e)
        {
            lock (mission)
            {
                lock (removeList)
                {
                    foreach (TimeTaskModel item in mission.Values)
                    {
                        if (item.time <= DateTime.Now.Ticks && !removeList.Contains(item.id))
                        {
                            item.Run();
                            removeList.Add(item.id);
                        }
                    }
                    TimeTaskModel outItem;
                    foreach (string item in removeList)
                    {
                        mission.TryRemove(item, out outItem);
                    }
                    removeList.Clear();
                }
            }
        }


        /// <summary>
        /// 任务调用 毫秒
        /// </summary>
        /// <param name="task"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public string Schedule(TimeEvent task, long delay)
        {
            //毫秒转微秒
            return Scheulemms(task, delay * 10000);
        }


        /// <summary>
        /// 任务调用 在指定时间
        /// </summary>
        /// <param name="task"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public string Schedule(TimeEvent task, DateTime time)
        {
            long t = time.Ticks - DateTime.Now.Ticks;
            t = t < 0 ? -t : t;
            return Scheulemms(task, t);
        }

        /// <summary>
        /// 任务调用 在指定微秒
        /// </summary>
        /// <param name="task"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public string TimeSchedule(TimeEvent task, long time)
        {
            long t = time - DateTime.Now.Ticks;
            t = t < 0 ? -t : t;
            return Scheulemms(task, t);
        }

        /// <summary>
        /// 微秒级时间轴
        /// </summary>
        /// <param name="task"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private string Scheulemms(TimeEvent task, long delay)
        {//10000000为一秒
            lock (mission)
            {
                string id = IDFactory.GetNewStringID();
                TimeTaskModel model = new TimeTaskModel(id, task, DateTime.Now.Ticks + delay);
                mission.TryAdd(id, model);
                return id;
            }
        }

        public void RemoveMission(string id)
        {
            lock (removeList)
            {
                removeList.Add(id);
            }
        }


    }
}
