using System;
using System.Collections.Generic;

namespace NetFrame
{
	public class UserToketPool
	{
		//System.Collections.Stack类表示对象的简单的后进先出非泛型集合
		private Stack<UserToken> pool;

		public UserToketPool (int max)
		{
			pool = new Stack<UserToken> (max);
		}
		/// <summary>
		/// 取出一个对象
		/// </summary>
		public UserToken pop()
		{
			return pool.Pop ();
		}

		/// <summary>
		/// 插入一个连接对象
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

