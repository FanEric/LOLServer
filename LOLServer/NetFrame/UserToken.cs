using System;
using System.Net.Sockets;

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
		/// 用户异步接收网络数据对象
		/// </summary>
		public SocketAsyncEventArgs receiveSAEA;
		/// <summary>
		/// 用户异步发送网络数据对象
		/// </summary>
		public SocketAsyncEventArgs sendSAEA;
		public UserToken ()
		{
			receiveSAEA = new SocketAsyncEventArgs ();
			sendSAEA = new SocketAsyncEventArgs ();
			receiveSAEA.UserToken = this;
			sendSAEA.UserToken = this;

		}

		public void Close()
		{
			
		}
	}
}

