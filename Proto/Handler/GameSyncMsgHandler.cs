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
            //room.Broadcast(msg);
            room.BroadcastWithSelect(msg, player.id);
        }

        public static void MsgGeneratePrefabHandler(ClientState c, ProtoBuf.IExtensible msgBase)
        {
            MsgGeneratePrefab msg = (MsgGeneratePrefab)msgBase;
            Player player = c.player;
            if (player == null) return;
            Room room = RoomManager.GetRoom(player.roomId);
            if (room == null) return;
            msg.targetId = player.id;
            //room.Broadcast(msg);
            room.BroadcastWithSelect(msg, player.id);
        }

        public static void MsgGeneratePrefabWDisHandler(ClientState c,ProtoBuf.IExtensible msgBase)
        {
            MsgGeneratePrefabWDis msg = (MsgGeneratePrefabWDis)msgBase;
            Player player = c.player;
            if (player == null) return;
            Room room = RoomManager.GetRoom(player.roomId);
            if (room == null) return;
            msg.camp = player.camp;
            room.BroadcastWithSelect(msg, player.id);
        }
    }
}
