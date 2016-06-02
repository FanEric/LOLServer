using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer
{
    class HandlerCenter : AbsHandlerCenter
    {
        public override void ClientConnect(UserToken token)
        {
            Console.WriteLine("有客户端连接");
        }

        public override void MessageReceive(UserToken token, object message)
        {
            throw new NotImplementedException();
        }

        public override void ClientClose(UserToken token, string error)
        {
            Console.WriteLine("有客户端断开连接");

        }
    }
}
