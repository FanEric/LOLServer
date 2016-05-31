using System;
using System.Collections.Generic;

namespace NetFrame
{
	public class UserToketPool
	{
		private Stack<UserToken> pool;

		public UserToketPool (int max)
		{
			pool = new Stack<UserToken> (max);
		}

		/// <summary>
		/// 取出一个对象－－创建连接
		/// </summary>
		public UserToken pop()
		{
			return pool.Pop ();
		}

		/// <summary>
		/// 插入一个连接对象－－释放连接
		/// </summary>
		/// <param name="token">Token.</param>
		public void push(UserToken token)
		{
			if (token != null)
				pool.Push (token);
		}

		public int Size{
			get{ return pool.Count;}
		}
	}
}

