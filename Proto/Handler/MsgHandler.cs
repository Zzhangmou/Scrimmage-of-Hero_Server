using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proto;

namespace Z_Scrimmage
{
    public partial class MsgHandler
    {
        public static void MsgPingHandler(ClientState c, ProtoBuf.IExtensible msgBase)
        {
            Console.WriteLine(c.socket.RemoteEndPoint + " MsgPing ");
            c.lastPingTime = NetManager.GetTimeStamp();
            MsgPong msgPong = new MsgPong();
            NetManager.Send(c, msgPong);
        }
    }
}
