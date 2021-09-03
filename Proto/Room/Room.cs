using System;
using System.Collections.Generic;
using proto;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z_Scrimmage
{
    class Room
    {
        //房间Id
        public int id;
        //房间最大玩家数
        public int maxPlayer = 6;
        //玩家列表
        public List<string> playerIds = new List<string>();
        public enum Status
        {
            Prepare = 0,
            Fight = 1
        }
        public Status status = Status.Prepare;

        public bool AddPlayer(string id)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player == null)
            {
                Console.WriteLine("房间加入玩家失败 player为null");
                return false;
            }
            if (playerIds.Contains(id))
            {
                Console.WriteLine("房间加入玩家失败 player已存在此房间");
                return false;
            }
            //加入列表
            playerIds.Add(id);
            //设置玩家数据
            player.camp = SwitchCamp();
            player.roomId = this.id;

            //广播
            Broadcast(new MsgEnterMatch() { currentMatchNum = playerIds.Count });

            return true;
        }

        public bool RemovePlayer(string id)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player == null)
            {
                Console.WriteLine("房间移除玩家失败 player为null");
                return false;
            }
            if (!playerIds.Contains(id))
            {
                Console.WriteLine("房间移除玩家失败 player不存在此房间");
                return false;
            }
            playerIds.Remove(id);
            player.camp = 0;
            player.roomId = -1;
            player.heroId = -1;

            //房间为空
            if (playerIds.Count == 0)
            {
                RoomManager.RemoveRoom(this.id);
            }

            Broadcast(new MsgLeaveMatch() { currentMatchNum = playerIds.Count });

            return true;
        }

        private int SwitchCamp()
        {
            int count1 = 0;
            int count2 = 0;
            foreach (string id in playerIds)
            {
                Player player = PlayerManager.GetPlayer(id);
                if (player.camp == 1)
                    count1++;
                if (player.camp == 2)
                    count2++;
            }
            if (count1 <= count2)
                return 1;
            else
                return 2;
        }

        //广播消息
        public void Broadcast(ProtoBuf.IExtensible msg)
        {
            foreach (string id in playerIds)
            {
                Player player = PlayerManager.GetPlayer(id);
                player.Send(msg);
            }
        }
    }
}