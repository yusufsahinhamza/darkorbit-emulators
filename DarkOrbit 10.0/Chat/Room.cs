using Ow.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Chat
{
    class Room
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public static Dictionary<Int32, Room> Rooms = new Dictionary<Int32, Room>();

        public Room(int id, int index, string name)
        {
            Id = id;
            Index = index;
            Name = name;
        }

        public static void AddRooms()
        {
            QueryManager.LoadChatRooms();
        }

        public override string ToString()
        {
            return Id + "|" + Name + "|" + Index + "|-1|0|0}";
        }
    }
}
