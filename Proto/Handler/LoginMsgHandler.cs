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
        public static void MsgRegisterHandler(ClientState c, ProtoBuf.IExtensible msgBase)
        {
            MsgRegister msg = (MsgRegister)msgBase;
            //注册
            if (DbManager.Register(msg.id, msg.pw))
            {
                DbManager.CreatePlayer(msg.id,msg.userName);
                msg.result = 0;
            }
            else
            {
                msg.result = -1;
            }
            NetManager.Send(c, msg);
        }

        public static void MsgLoginHandler(ClientState c,ProtoBuf.IExtensible msgBase)
        {
            MsgLogin msg = (MsgLogin)msgBase;
            //检查密码
            if (!DbManager.CheckPassword(msg.id, msg.pw))
            {
                msg.result = -1;
                NetManager.Send(c, msg);
                return;
            }
            //不允许再次登录
            if (c.player != null)
            {
                msg.result = -2;
                NetManager.Send(c, msg);
                return;
            }
            if (PlayerManager.IsOnLine(msg.id))
            {
                Player lastPlayer = PlayerManager.GetPlayer(msg.id);
                MsgKick msgKick = new MsgKick();
                msgKick.reason = 0;
                lastPlayer.Send(msgKick);

                //断开连接
                NetManager.Close(lastPlayer.state);
            }
            //获取玩家数据
            PlayerData playerData = DbManager.GetPlayerData(msg.id);
            if (playerData == null)
            {
                msg.result = -3;
                NetManager.Send(c, msg);
                return;
            }
            //创建player
            Player player = new Player(c);
            player.id = msg.id;
            player.data = playerData;
            PlayerManager.AddPlayer(msg.id, player);
            c.player = player;
            //返回数据
            msg.result = 0;
            player.Send(msg);
        }
    }
}
