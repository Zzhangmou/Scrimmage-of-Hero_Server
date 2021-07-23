using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z_Scrimmage
{
    public partial class EventHandler
    {
        public static void OnDisconnect(ClientState c)
        {
            Console.WriteLine(c.socket.RemoteEndPoint + " Close ");
        }

        public static void OnTimer()
        {
            CheckPing();
        }
        public static void CheckPing()
        {
            long timeNow = NetManager.GetTimeStamp();
            foreach (ClientState s in NetManager.clients.Values)
            {
                if (timeNow - s.lastPingTime > NetManager.pingInterval * 4)
                {
                    Console.WriteLine(DateTime.Now + "  Ping超时 " + s.socket.RemoteEndPoint.ToString());
                    NetManager.Close(s);
                    return;
                }
            }
        }
    }
}
