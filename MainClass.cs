using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;
using proto;

namespace Z_Scrimmage
{
    class MainClass
    {
        public static void Main()
        {
            if (!DbManager.Connect("game", "127.0.0.1", 3306, "root", ""))
                return;
            NetManager.StartLoop(8080);
        }
    }
}
