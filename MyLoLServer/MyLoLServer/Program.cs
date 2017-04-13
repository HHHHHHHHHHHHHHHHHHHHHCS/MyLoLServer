using MyLoLServer;
using NetFrame;
using NetFrame.auto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LolServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            //服务器初始化
            ServerStart ss = new ServerStart(1000);
            ss.center = new HandlerCenter();

            ss.Start(6650);

            Console.WriteLine("服务器启动成功!");

            string newStr = null;
            while (true)
            {
                newStr = Console.ReadLine();
                if(newStr == "-q" || newStr == "-Q")
                {
                    ss.CloseServer();
                    Console.WriteLine("终止服务器运行!");
                    break;
                }
            }
        }
    }
}
