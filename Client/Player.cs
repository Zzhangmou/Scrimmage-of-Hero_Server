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

        #region 临时数据
        //坐标 旋转
        public float x;
        public float y;
        public float z;
        public float ex;
        public float ey;
        public float ez;
        //房间Id
        public int roomId = -1;
        //阵营
        public int camp = 0;
        //选择人物
        public int heroId = -1;
        //选择地图
        public int mapId = -1;
        //生命值
        //...
        #endregion

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
