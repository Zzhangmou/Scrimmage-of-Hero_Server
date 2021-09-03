using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z_Scrimmage
{
    class RoomManager
    {
        private static int maxId = 1;
        public static Dictionary<int, Room> rooms = new Dictionary<int, Room>();

        //获取房间
        public static Room GetRoom(int id)
        {
            if (rooms.ContainsKey(id))
                return rooms[id];
            return null;
        }
        //创建房间
        public static Room CreateRoom()
        {
            maxId++;
            Room room = new Room();
            room.id = maxId;
            rooms.Add(room.id, room);
            return room;
        }
        //加入房间
        public static Room AddRoom()
        {
            //判断有无房间
            if (rooms.Count == 0)
                CreateRoom();
            //寻找可进入的房间
            foreach (int id in rooms.Keys)
            {
                Room room = GetRoom(id);
                if (room.playerIds.Count < room.maxPlayer)
                    return room;
            }
            return CreateRoom();
        }
        //删除房间
        public static bool RemoveRoom(int id)
        {
            rooms.Remove(id);
            return true;
        }
    }
}
