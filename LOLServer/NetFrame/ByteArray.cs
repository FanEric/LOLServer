using System;
using System.IO;

namespace NetFrame
{
	/// <summary>
	/// 将数据写入二进制
	/// </summary>
	public class ByteArray
	{
		MemoryStream ms = new MemoryStream();
		BinaryWriter bw;
		BinaryReader br;

		public ByteArray()
		{
			bw = new BinaryWriter (ms);
			br = new BinaryReader (ms);
		}

		/// <summary>
		/// 支持传入初始数据的构造
		/// </summary>
		/// <param name="buff">Buff.</param>
		public ByteArray (byte[] buff)
		{
			ms = new MemoryStream (buff);
			bw = new BinaryWriter (ms);
			br = new BinaryReader (ms);
		}

		/// <summary>
		/// 获取当前数据读取到的下标位置
		/// </summary>
		/// <value>The position.</value>
		public int Position{get{ return (int)ms.Position;}}

		/// <summary>
		/// 获取当前数据长度
		/// </summary>
		/// <value>The length.</value>
		public int Length{get{ return (int)ms.Length;}}

		/// <summary>
		/// 当前是否还有数据可以读取
		/// </summary>
		/// <value><c>true</c> if readable; otherwise, <c>false</c>.</value>
		public bool Readable{get{ return ms.Length > ms.Position;}}


		public void write(int value){
			bw.Write (value);
		}

		public void write(byte value){
			bw.Write (value);
		}

		public void write(bool value){
			bw.Write (value);
		}

		public void write(string value){
			bw.Write (value);
		}

		public void write(byte[] value){
			bw.Write (value);
		}

		public void write(double value){
			bw.Write (value);
		}

		public void write(float value){
			bw.Write (value);
		}

		public void write(long value){
			bw.Write (value);
		}

		public void read(out int value)
		{
			value = br.ReadInt32 ();
		}
		public void read(out byte value)
		{
			value = br.ReadByte ();
		}

		public void read(out bool value)
		{
			value = br.ReadBoolean ();
		}

		public void read(out string value)
		{
			value = br.ReadString ();
		}

		public void read(out byte[] value, int length)
		{
			value = br.ReadBytes (length);
		}

		public void read(out double value)
		{
			value = br.ReadDouble ();
		}

		public void read(out float value)
		{
			value = br.ReadSingle ();
		}

		public void read(out long value)
		{
			value = br.ReadInt64 ();
		}

		public void reposition()
		{
			ms.Position = 0;
		}

		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns>The buff.</returns>
		public byte[] getBuff()
		{
			byte[] result = new byte[ms.Length];
			Buffer.BlockCopy (ms.GetBuffer (), 0, result, 0, (int)ms.Length);
			return result;
		}

		public void Close()
		{
			bw.Close ();
			br.Close ();
			ms.Close ();
		}
	}
}

