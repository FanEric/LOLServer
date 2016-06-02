using System;

namespace NetFrame.auto
{
	public class MessageEncoding
	{
		public MessageEncoding ()
		{
		}

		/// <summary>
		/// 消息体序列化
		/// </summary>
		/// <param name="value">Value.</param>
		public static byte[] Encode(object value)
		{
			SocketModel model = value as SocketModel;
			ByteArray ba = new ByteArray ();
			ba.write (model.type);
			ba.write (model.area);
			ba.write (model.command);
			if (model != null) {
				ba.write(SerializeUtil.encode(model.message));
			}
			byte[] result = ba.getBuff ();
			ba.Close ();
            return result;
		}

		/// <summary>
		/// 消息体反序列化
		/// </summary>
		/// <param name="value">Value.</param>
        public static object Decode(byte[] value)
		{
			ByteArray ba = new ByteArray (value);
			SocketModel model = new SocketModel ();
			byte type;
			int area;
			int command;
			//从数据中读取三层协议，读取顺序必须和写入顺序保持一致
			ba.read (out type);
			ba.read (out area);
			ba.read (out command);
			model.type = type;
			model.area = area;
			model.command = command;
			//判断读取完协议后是否还有数据需要读取，是则说明有消息体，进行读取
			if (ba.Readable) {
				byte[] message;
				ba.read (out message, ba.Length - ba.Position);
				model.message = SerializeUtil.decode (message);
			}
			ba.Close ();
			return model;
		}
	}
}

