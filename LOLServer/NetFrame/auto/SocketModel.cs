using System;

namespace NetFrame.auto
{
	public class SocketModel
	{
		/// <summary>
		/// 一级协议，用于区分所属模块
		/// </summary>
		/// <value>The type.</value>
		public byte type{ get; set;}
		/// <summary>
		/// 二级协议，用于区分模块下属子模块
		/// </summary>
		/// <value>The area.</value>
		public int area{ get; set;}
		/// <summary>
		/// 三级协议， 用于区分当前处理逻辑功能
		/// </summary>
		/// <value>The command.</value>
		public int command{ get; set;}
		/// <summary>
		/// 消息体，当前需要处理的主体数据
		/// </summary>
		/// <value>The message.</value>
		public object message{ get; set;}

		public SocketModel ()
		{
		}

		public SocketModel (byte t, int a, int c, object m)
		{
			type = t;
			area = a;
			command = c;
			message = m;
		}

		public T GetMessage<T>()
		{
			return (T)message;
		}
	}
}

