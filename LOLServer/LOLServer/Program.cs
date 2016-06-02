using NetFrame;
using NetFrame.auto;
using System;
using System.Collections;

namespace LOLServer
{
	class MainClass
	{
        public static void Main()
        {
            int port = 6650;
            ServerStart ss = new ServerStart(9000);
            ss.encode = MessageEncoding.Encode;
            ss.decode = MessageEncoding.Decode;
            ss.center = new HandlerCenter();
            ss.Start(port);
            Console.WriteLine("服务器启动成功--> 端口号: {0}", port);
            while (true) { }
        }
	}
}
