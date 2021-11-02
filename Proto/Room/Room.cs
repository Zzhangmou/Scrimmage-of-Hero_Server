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
        public int preparedNum = 0;
        //房间最大玩家数
        public int maxPlayer = 1;
        //玩家列表
        public List<string> playerIds = new List<string>();

        public int count1Death = 0;
        public int count2Death = 0;
        /// <summary>
        /// 添加玩家
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            Console.WriteLine(player.data.userName + "加入房间匹配，所选人物id：" + player.heroId);

            //广播
            Broadcast(new MsgEnterMatch() { currentMatchNum = playerIds.Count });

            //如果房间人满 则开始游戏
            if (playerIds.Count == maxPlayer)
            {
                Console.WriteLine("开始游戏");
                SendAllPlayerInfo();
            }
            return true;
        }
        /// <summary>
        /// 移除玩家
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            player.isDead = false;

            //房间为空
            if (playerIds.Count == 0)
            {
                RoomManager.RemoveRoom(this.id);
            }

            return true;
        }
        /// <summary>
        /// 分配阵营
        /// </summary>
        /// <returns></returns>
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

        public void SendAllPlayerInfo()
        {
            MsgGetRoomInfo msg = new MsgGetRoomInfo();
            for (int i = 0; i < playerIds.Count; i++)
            {
                Player player = PlayerManager.GetPlayer(playerIds[i]);
                //组装协议
                msg.players.Add(new PlayerInfo()
                {
                    id = player.id,
                    camp = player.camp,
                    heroId = player.heroId,
                    userName = player.data.userName
                });
            }
            //发送协议
            foreach (string id in playerIds)
            {
                Player player = PlayerManager.GetPlayer(id);
                msg.currentCamp = player.camp;
                msg.userHeroId = player.heroId;
                player.Send(msg);
            }

            //Broadcast(msg);
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
        public void BroadcastWithSelect(ProtoBuf.IExtensible msg, string sendId)
        {
            foreach (string id in playerIds)
            {
                if (id != sendId)
                {
                    Player player = PlayerManager.GetPlayer(id);
                    player.Send(msg);
                }
            }
        }
        public void RemoveAllPlayer()
        {
            for (int i = playerIds.Count - 1; i >= 0; i--)
            {
                RemovePlayer(playerIds[i]);
            }
        }
        public void CheckPlayerCampStatus()
        {
            if (count1Death >= maxPlayer / 2)
            {
                MsgBattleResult msg = new MsgBattleResult()
                {
                    winCamp = 2,
                    loseCamp = 1
                };
                Broadcast(msg);
                RemoveAllPlayer();
            }
            if (count2Death >= maxPlayer / 2)
            {
                MsgBattleResult msg = new MsgBattleResult()
                {
                    winCamp = 1,
                    loseCamp = 2
                };
                Broadcast(msg);
                RemoveAllPlayer();
            }
        }
    }
}