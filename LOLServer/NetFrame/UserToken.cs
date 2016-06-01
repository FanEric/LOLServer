using System;
using System.Net.Sockets;
using System.Collections.Generic;

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

        public LengthEncode LE;
        public LengthDecode LD;
        public Encode encode;
        public Decode decode;

        public delegate void SendProcess(SocketAsyncEventArgs e);
        public SendProcess sendProcess;

		public delegate void CloseProcess(UserToken token, string error);
		public CloseProcess closeProcess;

		public AbsHandlerCenter center;

        List<byte> cache = new List<byte>();

        private bool isReading = false;
        private bool isWriting = false;

        Queue<byte[]> writeQueue = new Queue<byte[]>();

        public UserToken ()
		{
			receiveSAEA = new SocketAsyncEventArgs ();
			sendSAEA = new SocketAsyncEventArgs ();
			receiveSAEA.UserToken = this;
			sendSAEA.UserToken = this;

		}

        /// <summary>
        /// 网络消息达到
        /// </summary>
        /// <param name="buff"></param>
        public void Recevie(byte[] buff)
        {
            //将消息写入缓存
            cache.AddRange(buff);

            if (!isReading)
            {
                isReading = true;
                OnData();
            }
        }

        /// <summary>
        /// 缓存中有数据
        /// </summary>
        void OnData()
        {
            //解码消息存储对象
            byte[] buff = null;
            //当粘包解码器存在的时候进行粘包处理
            if (LD != null)
            {
                buff = LD(ref cache);
                //消息未接收全，退出数据处理，等待下次消息到达
                if (buff == null)
                {
                    isReading = false;
                    return;
                }
            }
            else {
                //缓存中没有数据，直接跳出数据处理，等待下次消息到达
                if (cache.Count == 0)
                {
                    isReading = false;
                    return;
                }
            }
            //反序列化方法是否存在
            if (decode == null) throw new Exception("message decode process is null");
            //进行消息反序列化
            object message = decode(buff);

            //TODO 通知应用层有消息到达
			center.MessageReceive(this, message);
            //尾递归，防止在消息处理过程中有其他消息达到而没有处理
            OnData();
        }

        public void Write(byte[] value)
        {
			if (conn == null) {
				closeProcess (this, "此连接已断开");
				return;  //次连接已断开
			}

            writeQueue.Enqueue(value);
            if (!isWriting)
            {
                isWriting = true;
                OnWrite();
            }
        }

        public void OnWrite()
        {
            //判断发送消息队列是否有消息
            if (writeQueue.Count == 0) { isWriting = false; return; }
            //取出第一条待发消息
            byte[] buff = writeQueue.Dequeue();
            //设置消息发送异步对象的发送数据缓冲区数据
            sendSAEA.SetBuffer(buff, 0, buff.Length);
            //开始异步发送
            bool result = conn.SendAsync(sendSAEA);
            //是否挂起
            if (!result)
            {
                sendProcess(sendSAEA);
            }
        }

        /// <summary>
        /// 发送完成
        /// </summary>
        public void Writed()
        {
            //与OnData()尾递归同理
            OnWrite();
        }

		public void Close()
		{
            try
            {
                writeQueue.Clear();
                cache.Clear();
                isReading = false;
                isWriting = false;
                conn.Shutdown(SocketShutdown.Both);
                conn.Close();
                conn = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
	}
}

