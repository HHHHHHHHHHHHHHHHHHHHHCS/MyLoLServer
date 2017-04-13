using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    public class UserTokenPool
    {
        private Stack<UserToken> pool; 

        public UserTokenPool(int max)
        {
            pool = new Stack<UserToken>(max);
        }

        /// <summary>
        /// 删除一个连接对象 --创建连接
        /// </summary>
        /// <returns></returns>
        public UserToken Pop()
        {
            return pool.Pop();
        }

        /// <summary>
        /// 插入一个连接对象 --释放连接
        /// </summary>
        /// <param name="token">插入的对象</param>
        public void Push(UserToken token)
        {
            if(token!=null)
            {
                pool.Push(token);
            }
        }

        /// <summary>
        /// 返回全部连接对象的数量
        /// </summary>
        public int Size
        {
            get
            {
                return pool.Count;
            }
        }
    }
}
