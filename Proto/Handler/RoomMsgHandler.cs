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
        public static void MsgGetUserInfoHandler(ClientState c, ProtoBuf.IExtensible msgBase)
        {
            MsgGetUserInfo msg = (MsgGetUserInfo)msgBase;
            Player player = c.player;
            if (player == null) return;
            msg.userName = player.data.userName;
            msg.win = player.data.winNum;
            msg.lose = player.data.loseNum;

            player.Send(msg);
        }
    }
}
