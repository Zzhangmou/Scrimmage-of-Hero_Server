using proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z_Scrimmage
{
    public partial class MsgHandler
    {
        public static void MsgHitHandler(ClientState c, ProtoBuf.IExtensible msgBase)
        {
            MsgHit msg = (MsgHit)msgBase;
            Player player = c.player;
            if (player == null) return;
            Room room = RoomManager.GetRoom(player.roomId);
            if (room == null) return;

            msg.attackId = player.id;
            room.Broadcast(msg);
            //room.BroadcastWithSelect(msg, player.id);
        }
        public static void MsgDeathHandler(ClientState c, ProtoBuf.IExtensible msgBase)
        {
            Console.WriteLine("死亡");
            MsgDeath msg = (MsgDeath)msgBase;
            Player player = c.player;
            if (player == null) return;
            Room room = RoomManager.GetRoom(player.roomId);
            if (room == null) return;
            if (msg.belongCamp == 1)
                room.count1Death++;
            if (msg.belongCamp == 2)
                room.count2Death++;
            room.CheckPlayerCampStatus();
        }
    }
}
