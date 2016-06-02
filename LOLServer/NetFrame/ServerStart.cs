using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Threading;

namespace NetFrame
{
	public class ServerStart
	{
		//test
		Socket server;//服务器socket监听对象   
		int maxClient;//最大客户端连接数
		Semaphore acceptClients;//限制可同时访问某一资源或资源池的线程数
		UserToketPool pool;

        public LengthEncode LE;
        public LengthDecode LD;
        public Encode encode;
        public Decode decode;

		/// <summary>
		/// 消息处理中心，由外部应用传入
		/// </summary>
		public AbsHandlerCenter center;

        /// <summary>
        /// 初始化通讯监听
        /// </summary>
        /// <param name="port">最大连接数.</param>
        public ServerStart (int max)
		{
			//实例化监听对象
			server = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//设定服务器最大连接数
			maxClient = max;

		}

		public void Start(int port)
		{
			pool = new UserToketPool (maxClient);
			//连接信号量
			acceptClients = new Semaphore (maxClient, maxClient);//初始化的时候就获取最大的连接数
			for (int i = 0; i < maxClient; i++) {
				UserToken token = new UserToken ();
				//初始化token的信息
				token.receiveSAEA.Completed += new EventHandler<SocketAsyncEventArgs> (IOCompleted);
				token.sendSAEA.Completed += new EventHandler<SocketAsyncEventArgs> (IOCompleted);
				token.LD = LD;
				token.LE = LE;
				token.encode = encode;
				token.decode = decode;
				token.sendProcess = ProcessSend;
				token.closeProcess = ClientClose;
				token.center = center;
				pool.push (token);
			}


            try  //防止服务器重复启动
            {
                //监听当前服务器网卡所有可用IP地址的端口
                server.Bind(new IPEndPoint(IPAddress.Any, port));
                //置于监听状态
                server.Listen(10);
                StartAccept(null);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
			
		}

		/// <summary>
		/// 开始客户端连接监听
		/// </summary>
		/// <param name="e">E.</param>
		public void StartAccept(SocketAsyncEventArgs e)
		{
			//如果当前传入为空，说明调用新的客户端连接监听事件
			//否则移除当前客户端连接
			if (e == null) {
				e = new SocketAsyncEventArgs ();
				e.Completed += new EventHandler<SocketAsyncEventArgs> (AcceptCompleted);
			} else {
				e.AcceptSocket = null;
			}

            //信号量-1
			acceptClients.WaitOne ();
			bool result = server.AcceptAsync (e);
			//判断异步事件是否挂起，没挂起说明立刻执行完成，直接处理事件
			//否则会在处理完成后触发AcceptCompleted事件

			//如果 I/O 操作挂起，将返回 true。操作完成时，将引发 e 参数的 SocketAsyncEventArgs.Completed 事件。
			//如果 I/O 操作同步完成，将返回 false。将不会引发 e 参数的 SocketAsyncEventArgs.Completed 事件，并且可能在方法调用返回后立即检查作为参数传递的 e 对象以检索操作的结果。
			if (!result) {
				ProcessAccept (e);
			}
				
		}

		public void AcceptCompleted(Object sender, SocketAsyncEventArgs e)
		{
			ProcessAccept (e);
		}

		public void ProcessAccept(SocketAsyncEventArgs e)
		{
			//从连接对象池取出连接对象，供用户使用
			UserToken token = pool.pop ();
			token.conn = e.AcceptSocket;
			//TODO 通知应用层，有客户端连接
			center.ClientConnect(token);
			//开启消息到达监听
			StartReceive(token);
			//释放当前异步对象
			StartAccept(e);
		}

		public void IOCompleted(Object sender, SocketAsyncEventArgs e)
		{
			if (e.LastOperation == SocketAsyncOperation.Receive) {
				ProcessReceive (e);
			} else {
				ProcessSend (e);
			}
		}
            
		public void StartReceive(UserToken token)
		{
            try {
                //用户连接对象 开启异步接收数据
                bool result = token.conn.ReceiveAsync(token.receiveSAEA);
                //异步事件是否挂起
                if (!result)
                {
                    ProcessReceive(token.receiveSAEA);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
		}

		public void ProcessReceive(SocketAsyncEventArgs e)
		{
			UserToken token = e.UserToken as UserToken;

			//判断网络消息接收是否成功
			if (token.receiveSAEA.BytesTransferred > 0 && token.receiveSAEA.SocketError == SocketError.Success) {
				byte[] message = new byte[token.receiveSAEA.BytesTransferred];
				Buffer.BlockCopy (token.receiveSAEA.Buffer, 0, message, 0, token.receiveSAEA.BytesTransferred);
                //处理接收到的消息
                token.Recevie(message);
				StartReceive (token);
			} else {
				if (token.receiveSAEA.SocketError != SocketError.Success) {
                    ClientClose(token, token.receiveSAEA.SocketError.ToString());
				}
                else //消息为0且没有错误信息的话判断为客户端主动断开连接
                {
                    ClientClose(token, "客户端主动断开连接");
				}
			}
		}

		public void ProcessSend(SocketAsyncEventArgs e)
		{
            UserToken token = e.UserToken as UserToken;
            if (e.SocketError != SocketError.Success)
            {
                ClientClose(token, e.SocketError.ToString());
            }
            else {
                //消息发送成功，回调处理
                token.Writed();
            }
        }

		/// <summary>
		/// 客户端断开连接
		/// </summary>
		/// <param name="token">断开连接的用户对象.</param>
		/// <param name="error">断开连接的错误编码.</param>
		public void ClientClose(UserToken token, string error)
		{
			if (token.conn != null) {
				lock (token) {
					//通知应用层面，客户端断开连接了
					center.ClientClose(token, error);
					token.Close();
					pool.push (token);
					//加回一个信号量，供其他用户使用
					acceptClients.Release ();
				}
			}
		}
	}
}

