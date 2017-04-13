using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetFrame
{
    public class ServerStart
    {
        Socket server;//服务器socket监听对象

        int maxClient;//最大客户端连接数
        Semaphore acceptClients;//连接的信号量
        UserTokenPool pool;

        public LengthEncode LE;
        public LengthDecode LD;
        public Encode encode;
        public Decode decode;

        /// <summary>
        /// 消息处理中心，由外部应用传入
        /// </summary>
        public AbsHandlerCenter center;


        /// <summary>
        /// 初始化通信监听
        /// </summary>
        /// <param name="port">监听端口</param>
        /// <param name="_maxClient">最大监听数量</param> 
        public ServerStart(int _maxClient)
        {
            //实例化监听对象
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //设定服务器最大连接人数
            maxClient = _maxClient;

        }

        /// <summary>
        /// 服务器启动 开始监听
        /// </summary>
        /// <param name="port">服务器端口</param>
        public void Start(int port)
        {
            if (encode == null)
            {
                encode = MessageEncoding.Encode;
            }
            if (decode == null)
            {
                decode = MessageEncoding.Decode;
            }
            if (LE == null)
            {
                LE = LengthEncoding.Encode;
            }
            if (LD == null)
            {
                LD = LengthEncoding.Decode;
            }


            //创建连接池
            pool = new UserTokenPool(maxClient);
            //连接的信号量
            acceptClients = new Semaphore(maxClient, maxClient);
            for (int i = 0; i < maxClient; i++)
            {
                UserToken newToken = new UserToken();
                //初始化token信息
                newToken.receiveSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Comleted);
                newToken.sendSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Comleted);
                newToken.LE = LE;
                newToken.LD = LD;
                newToken.encode = encode;
                newToken.decode = decode;
                newToken.sendProcess = ProcessSend;
                newToken.closeProcess = ClientClose;
                newToken.center = center;
                pool.Push(newToken);
            }


            //监听当前服务器网卡所有可用的IP地址的port端口
            //外网IP 内网IP192.168.x.x 本机IP 127.0.0.1
            server.Bind(new IPEndPoint(IPAddress.Any, port));
            //置于一定数量为等待监听状态 
            server.Listen(10);
            StartAccept(null);
        }

        /// <summary>
        /// 开始客户端连接监听
        /// </summary>
        public void StartAccept(SocketAsyncEventArgs e)
        {
            //如果当前传入为空 说明调用新的客户端连接监听事件 否则的话 移除当前客户端连接
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Comleted);
            }
            else
            {
                e.AcceptSocket = null;
            }
            //信号量-1
            acceptClients.WaitOne();
            bool result = server.AcceptAsync(e);//返回值表示是否即刻就完成了

            //判断异步事件是否挂起 没挂起说明立刻执行 直接处理事件 
            //如果挂起则会在处理完成后触发
            if (!result)
            {
                ProcessAccept(e);
            }
        }

        public void ProcessAccept(SocketAsyncEventArgs e)
        {
            //从连接对象池取出一个连接对象 供新用户使用
            UserToken token = pool.Pop();
            token.conn = e.AcceptSocket;
            //通知应用层有客户端连接
            center.ClientConnect((UserToken)e.UserToken);
            //开启消息到达监听
            StartReceive(token);
            //释放当前异步对象
            StartAccept(e);
        }

        public void Accept_Comleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        public void StartReceive(UserToken token)
        {
            //用户对象连接 开启异步数据接受
            bool result = token.conn.ReceiveAsync(token.receiveSAEA);
            //异步事件是否挂起
            if (!result)
            {
                ProcessReceive(token.receiveSAEA);
            }
        }

        /// <summary>
        /// 数据发送接受的委托处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IO_Comleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Receive)
            {
                ProcessReceive(e);
            }
            else if (e.LastOperation == SocketAsyncOperation.Send)
            {
                ProcessSend(e);
            }
        }

        /// <summary>
        /// 接受信息
        /// </summary>
        /// <param name="e"></param>
        public void ProcessReceive(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if (token.receiveSAEA.BytesTransferred > 0
                && token.receiveSAEA.SocketError == SocketError.Success)
            {
                byte[] message = new byte[token.receiveSAEA.BytesTransferred];
                //将网络消息拷贝到自定义数组
                Buffer.BlockCopy(token.receiveSAEA.Buffer, 0
                    , message, 0, token.receiveSAEA.BytesTransferred);
                //处理接受收到的消息
                token.Receive(message);
                StartReceive(token);
            }
            else
            {
                if (token.receiveSAEA.SocketError != SocketError.Success)
                {
                    ClientClose(token, token.receiveSAEA.SocketError.ToString());
                }
                else
                {
                    ClientClose(token, "客户端主动断开连接");
                }
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="e"></param>
        public void ProcessSend(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if (e.SocketError != SocketError.Success)
            {//如果网络错误直接强制关闭客户端
                ClientClose(token, e.SocketError.ToString());
            }
            else
            {//消息发送成功，回调成功
                token.Writed();
            }
        }


        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="token">断开连接的用户对象</param>
        /// <param name="error">断开连接的错误编码</param>
        public void ClientClose(UserToken token, string error)
        {
            if (token.conn != null)
            {
                lock (token)
                {
                    //通知应用层面 
                    center.ClientClose(token, error);
                    //客户端断开连接
                    token.Close();
                    //塞回连接对象
                    pool.Push(token);
                    //加回一个信号量，供其他用户使用
                    acceptClients.Release();
                }
            }
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public void CloseServer()
        {
            //server.Close();
            server.Dispose();
        }


    }
}
