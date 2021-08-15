using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z_Scrimmage
{
    /// <summary>
    /// 游戏角色类
    /// </summary>
    public class Player
    {
        public string id;
        public ClientState state;
        //数据库数据
        public PlayerData data;

        public Player(ClientState state)
        {
            this.state = state;
        }
        //发送信息
        public void Send(ProtoBuf.IExtensible msgBase)
        {
            NetManager.Send(state, msgBase);
        }
    }
}
