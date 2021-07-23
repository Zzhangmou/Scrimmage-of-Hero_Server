using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Z_Scrimmage
{
    /// <summary>
    /// 客户端信息类
    /// </summary>
    public class ClientState
    {
        public Socket socket;
        public ByteArray readBuff = new ByteArray();

        public long lastPingTime = 0;

        public Player player;
    }
}
