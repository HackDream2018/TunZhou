using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocketService
{
    class Program
    {
        static void Main(string[] args)
        {
            //创建客户端连接池，存储客户端连接
            List<IWebSocketConnection> dic_Sockets = new List<IWebSocketConnection>();
            //创建开启Socket本地服务
            WebSocketServer server = new WebSocketServer("ws://0.0.0.0:30000");//监听所有的的地址  
            //出错后进行重启  
            server.RestartAfterListenError = true;
            //开始监听  
            server.Start(socket =>
            {
                //获取客户端IP
                string clientUrl = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
                //连接建立事件  
                socket.OnOpen = () =>
                {
                    //存进连接池
                    dic_Sockets.Add(socket);
                    //输出连接信息
                    Console.WriteLine(DateTime.Now.ToString() + "|服务器:和客户端网页:" + clientUrl + " 建立WebSock连接！");
                };
                //连接关闭事件  
                socket.OnClose = () =>
                {
                    dic_Sockets.Remove(socket);
                    Console.WriteLine(DateTime.Now.ToString() + "|服务器:和客户端网页:" + clientUrl + " 断开WebSock连接！");
                };
                //接受客户端网页消息事件  
                socket.OnMessage = message =>
                {
                    Console.WriteLine(DateTime.Now.ToString() + "|服务器:【收到】来客户端网页:" + clientUrl + "的信息：\n" + message);
                    dic_Sockets.ToList().ForEach(s => s.Send(socket.ConnectionInfo.ClientIpAddress + ": " + message));
                };
            });
            ////关闭与客户端的所有的连接  
            //foreach (var item in dic_Sockets)
            //{
            //    if (item != null)
            //    {
            //        item.Close();
            //    }
            //}
            Console.ReadKey();
        }
    }
}
