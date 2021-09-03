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

        public static void MsgStartMatchHandler(ClientState c, ProtoBuf.IExtensible msgBase)
        {
            MsgStartMatch msg = (MsgStartMatch)msgBase;
            Player player = c.player;
            if (player == null) return;
            player.heroId = msg.heroId;
            Room room = RoomManager.AddRoom();
            room.AddPlayer(c.player.id);
        }

        public static void MsgLeaveMatchHandler(ClientState c, ProtoBuf.IExtensible msgBase)
        {
            Player player = c.player;
            if (player == null) return;
            Room room = RoomManager.GetRoom(player.roomId);
            if (room == null) return;
            room.RemovePlayer(player.id);
        }
    }
}
