using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    /// <summary>
    /// 用户连接信息对象 
    /// </summary>
    public class UserToken
    {
        /// <summary>
        /// 用户连接
        /// </summary>
        public Socket conn;
        /// <summary>
        /// 用户异步接受网络数据对象
        /// </summary>
        public SocketAsyncEventArgs receiveSAEA;
        /// <summary>
        /// 用户异步发送网络数据对象
        /// </summary>
        public SocketAsyncEventArgs sendSAEA;


        public LengthEncode LE;
        public LengthDecode LD;
        public Encode encode;
        public Decode decode;

        private List<byte> cache = new List<byte>();

        private bool isReading = false;
        private bool isWriting = false;
        private Queue<byte[]> writeQueue = new Queue<byte[]>();

        public delegate void SendProcess(SocketAsyncEventArgs e);

        public SendProcess sendProcess;

        public delegate void CloseProcess(UserToken token, string error);

        public CloseProcess closeProcess;

        public AbsHandlerCenter center;

        public UserToken()
        {
            receiveSAEA = new SocketAsyncEventArgs();
            sendSAEA = new SocketAsyncEventArgs();
            receiveSAEA.UserToken = this;
            sendSAEA.UserToken = this;
            //设置接受对象缓冲区大小
            receiveSAEA.SetBuffer(new byte[1024], 0, 1024);
        }

        /// <summary>
        /// 接受消息到达处理
        /// </summary>
        /// <param name="buff"></param>
        public void Receive(byte[] buff)
        {
            //将消息写入缓存
            cache.AddRange(buff);
            if(!isReading)
            {
                isReading = true;
                OnData();
            }
  
        }

        /// <summary>
        /// 缓存中有数据处理
        /// </summary>
        void OnData()
        {
            //解码消息存储对象
            byte[] buff = null;
            //当粘包解码器存在的时候 进行粘包处理
            if (LD != null)
            {
                buff = LD(ref cache);
                //消息为接受全 退出数据处理 等待下次消息到达
                if (buff == null)
                {
                    isReading = false;
                    return;
                }
            }
            else if(cache.Count==0)
            {//缓存区中没有数据 直接跳出消息处理 等待下次消息到达
                isReading = false;
                return;
            }
            else
            {
                buff = cache.ToArray();
                cache.Clear();
            }
            if(decode==null)
            {//消息解码器是一定要有的 否则抛出异常
                throw new Exception("message decode process is null!");
            }
            //进行消息解析
            object message = decode(buff);

            //通知应用层有消息到达
            center.MessageReceive(this, message);
            //尾递归 防止在消息处理过程中有其他消息到了 而没有经过处理
            OnData();
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="value">要发送的信息</param>
        public void Write(byte[] value)
        {
            if(conn==null)
            {//如果连接断开 则直接跳出 
                closeProcess(this, "调用已经断开的连接！");
                return;
            }
            //否则 将消息添加到队列
            writeQueue.Enqueue(value);
            if(!isWriting)
            {
                isWriting = true;
                OnWrite();
            }
        }

        /// <summary>
        /// 发送信息处理
        /// </summary>
        public void OnWrite()
        {
            //判断发骚那个消息队列是否有消息
            if(writeQueue.Count==0)
            {
                isWriting = false;
                return;
            }
            //取出第一条待发送信息
            byte[] buff = writeQueue.Dequeue();
            //设置消息发送异步对象的发送数据缓冲区
            sendSAEA.SetBuffer(buff, 0, buff.Length);
            //开启异步发送
            bool result = conn.SendAsync(sendSAEA);
            //是否挂起
            if(!result)
            {
                sendProcess(sendSAEA);
            }
        }

        /// <summary>
        /// 发送信息成功处理
        /// </summary>
        public void Writed()
        {
            //尾递归 将队列中的剩余信息发出去
            OnWrite();
        }


        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            try
            {
                writeQueue.Clear();
                cache.Clear();
                isReading = false;
                isWriting = false;
                conn.Shutdown(SocketShutdown.Both);
                conn.Close();
                conn = null;
                
            }
            catch (Exception e)
            {
                Console.WriteLine("客户端关闭失败，错误详情如下：");
                Console.WriteLine(e.Message);
            }
        }
    }
}
