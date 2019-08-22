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
        public int Id { get; set; }
        public string Name { get; set; }
        public int TabOrder { get; set; }
        public int CompanyId { get; set; }

        public static Dictionary<Int32, Room> Rooms = new Dictionary<Int32, Room>();

        public Room(int id, string name, int tabOrder, int companyId)
        {
            Id = id;
            Name = name;
            TabOrder = tabOrder;
            CompanyId = companyId;
        }

        public static void AddRooms()
        {
            ChatClient.LoadChatRooms();
        }

        public override string ToString()
        {
            return Id + "|" + Name + "|" + TabOrder + "|" + CompanyId + "|0|0}";
        }
    }
}
