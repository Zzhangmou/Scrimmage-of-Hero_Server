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
        public static void MsgSyncPosHandler(ClientState c, ProtoBuf.IExtensible msgBase)
        {
            MsgSyncPos msg = (MsgSyncPos)msgBase;
            Player player = c.player;
            if (player == null) return;
            Room room = RoomManager.GetRoom(player.roomId);
            if (room == null) return;

            msg.id = player.id;
            room.Broadcast(msg);
        }
    }
}
