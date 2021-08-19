using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Z_Scrimmage
{
    //网络管理器
    class NetManager
    {
        //监听Socket
        public static Socket listenfdSocket;
        //客户端Socket以及状态信息
        public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();
        //Select检查列表
        private static List<Socket> checkRead = new List<Socket>();
        //Ping间隔
        public static long pingInterval = 10;

        public static void StartLoop(int listenPort)
        {
            listenfdSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定
            IPAddress ipAdr = IPAddress.Parse("0.0.0.0");
            IPEndPoint ipEp = new IPEndPoint(ipAdr, listenPort);
            listenfdSocket.Bind(ipEp);
            //监听
            listenfdSocket.Listen(0);
            Console.WriteLine(DateTime.Now + " 服务器启动成功 ");

            //开始循环
            while (true)
            {
                ResetCheckRead();
                Socket.Select(checkRead, null, null, 1000);
                //检查可读对象
                for (int i = checkRead.Count - 1; i >= 0; i--)
                {
                    Socket s = checkRead[i];
                    if (s == listenfdSocket)
                        ReadListenfd(s);
                    else
                        ReadClientfd(s);
                }
                Timer();//超时
            }
        }

        private static void Timer()
        {
            MethodInfo mei = typeof(EventHandler).GetMethod("OnTimer");
            object[] ob = { };
            mei.Invoke(null, ob);
        }

        private static void ReadClientfd(Socket clientfdSocket)
        {
            ClientState state = clients[clientfdSocket];
            ByteArray readBuff = state.readBuff;
            //接收
            int count = 0;
            if (readBuff.remain <= 0)
            {
                OnReceiveData(state);
                readBuff.MoveBytes();
            }
            if (readBuff.remain <= 0)
            {
                Console.WriteLine("接收失败 ，接收数据超过了数组容量");
                Close(state);
                return;
            }
            try
            {
                count = clientfdSocket.Receive(readBuff.bytes, readBuff.writeIndex, readBuff.remain, 0);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("接收异常 " + ex.ToString());
                Close(state);
                return;
            }
            //客户端关闭
            if (count <= 0)
            {
                Console.WriteLine(DateTime.Now + " 客户端关闭 " + clientfdSocket.RemoteEndPoint.ToString());
                Close(state);
                return;
            }
            //消息处理
            readBuff.writeIndex += count;
            //处理二进制消息
            OnReceiveData(state);
            readBuff.CheckAndMoveBytes();
        }
        //关闭连接
        public static void Close(ClientState state)
        {
            MethodInfo mei = typeof(EventHandler).GetMethod("OnDisconnect");
            object[] ob = { state };
            mei.Invoke(null, ob);
            //关闭
            state.socket.Close();
            clients.Remove(state.socket);
        }
        //数据处理
        private static void OnReceiveData(ClientState state)
        {
            ByteArray readBuff = state.readBuff;
            //消息长度不够
            if (readBuff.Length <= 2) return;
            Int16 bodyLength = readBuff.ReadInt16();
            //消息体长度不够
            if (readBuff.Length < bodyLength) return;
            //解析协议名
            int nameCount;
            string protoName = ProtobufHelper.DecodeName(readBuff.bytes, readBuff.readIndex, out nameCount);
            if (protoName == "")
            {
                Console.WriteLine("解析协议名为空！！！");
                Close(state);
                return;
            }
            readBuff.readIndex += nameCount;
            //解析协议体
            int bodyCount = bodyLength - nameCount;
            ProtoBuf.IExtensible msgBase = ProtobufHelper.Decode(protoName, readBuff.bytes, readBuff.readIndex, bodyCount);
            readBuff.readIndex += bodyCount;
            readBuff.CheckAndMoveBytes();
            //分发消息  方法名+Handler
            MethodInfo mi = typeof(MsgHandler).GetMethod(protoName + "Handler");
            object[] o = { state, msgBase };
            Console.WriteLine(DateTime.Now + " 收到协议 " + protoName);
            if (mi != null)
                mi.Invoke(null, o);
            else
                Console.WriteLine("协议执行失败 " + protoName);
            //长度足够继续读取消息
            if (readBuff.Length > 2)
                OnReceiveData(state);
        }

        private static void ReadListenfd(Socket listenfdSocket)
        {
            try
            {
                Socket clientfd = listenfdSocket.Accept();
                Console.WriteLine(DateTime.Now + " 接收到 " + clientfd.RemoteEndPoint.ToString());
                ClientState state = new ClientState();
                state.socket = clientfd;
                state.lastPingTime = GetTimeStamp();
                clients.Add(clientfd, state);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("接收失败 " + ex.ToString());
            }
        }

        //重置
        private static void ResetCheckRead()
        {
            checkRead.Clear();
            checkRead.Add(listenfdSocket);
            foreach (ClientState s in clients.Values)
            {
                checkRead.Add(s.socket);
            }
        }
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        public static void Send(ClientState cs, ProtoBuf.IExtensible msg)
        {
            //判断
            if (cs == null) return;
            if (!cs.socket.Connected) return;
            //数据编码
            byte[] nameBytes = ProtobufHelper.EncodeName(msg);
            byte[] bodyBytes = ProtobufHelper.Encode(msg);
            int len = nameBytes.Length + bodyBytes.Length;
            byte[] sendBytes = new byte[2 + len];
            //组装长度
            sendBytes[0] = (byte)(len % 256);
            sendBytes[1] = (byte)(len / 256);
            //组装名字
            Array.Copy(nameBytes, 0, sendBytes, 2, nameBytes.Length);
            //组装消息体
            Array.Copy(bodyBytes, 0, sendBytes, 2 + nameBytes.Length, bodyBytes.Length);

            try
            {
                cs.socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, null, null);
            }
            catch (SocketException ex)
            {
                Console.WriteLine("发送失败 " + ex.ToString());
            }
        }
    }
}
