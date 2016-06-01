using System;
using System.Collections;

namespace LOLServer
{
	class MainClass
	{
		public static void Main()
		{

			//创建一个堆栈
			Stack myStack = new Stack();
			myStack.Push("The");//入栈
			myStack.Push("quick");
			myStack.Push("brown");
			myStack.Push("fox");


			// 打印集合中的值
			Console.Write("Stack values:");
			PrintValues(myStack, '\t');

			// 打印堆栈顶部的第一个元素，并将其移除
			Console.WriteLine("(Pop)\t\t{0}", myStack.Pop());

			//打印集合中的值
			Console.Write("Stack values:");
			PrintValues(myStack, '\t');

			// 打印堆栈顶部的第一个元素，并将其移除
			Console.WriteLine("(Pop)\t\t{0}", myStack.Pop());

			//打印集合中的值
			Console.Write("Stack values:");
			PrintValues(myStack, '\t');

			// 打印堆栈顶部的第一个元素
			Console.WriteLine("(Peek)\t\t{0}", myStack.Peek());

			// 打印集合中的值
			Console.Write("Stack values:");
			PrintValues(myStack, '\t');
			Console.Read();
		}


		public static void PrintValues(IEnumerable myCollection, char mySeparator)
		{
			foreach (Object obj in myCollection)
				Console.Write("{0}{1}", mySeparator, obj);
			Console.WriteLine();
		}
	}
}
