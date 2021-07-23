using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z_Scrimmage
{
    public class PlayerManager
    {
        //玩家列表
        private static Dictionary<string, Player> players = new Dictionary<string, Player>();
        /// <summary>
        /// 玩家是否在线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsOnLine(string id)
        {
            return players.ContainsKey(id);
        }
        /// <summary>
        /// 获取玩家
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Player GetPlayer(string id)
        {
            if (players.ContainsKey(id))
                return players[id];
            return null;
        }
        /// <summary>
        /// 添加玩家
        /// </summary>
        /// <param name="id"></param>
        /// <param name="player"></param>
        public static void AddPlayer(string id,Player player)
        {
            players.Add(id, player);
        }
        /// <summary>
        /// 移除玩家
        /// </summary>
        /// <param name="id"></param>
        public static void RemovePlayer(string id)
        {
            players.Remove(id);
        }
    }
}
