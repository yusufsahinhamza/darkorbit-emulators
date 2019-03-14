using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupPlayerTargetModule : command_i3O
    {
        public const short ID = 29238;

        public GroupPlayerShipModule ship;     
        public GroupPlayerInformationsModule information;     
        public String name = "";

        public GroupPlayerTargetModule(GroupPlayerShipModule ship, string name, GroupPlayerInformationsModule information)
        {
            this.ship = ship;
            this.name = name;
            this.information = information;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.write(ship.write());
            param1.write(information.write());
            param1.writeUTF(this.name);
            return param1.Message.ToArray();
        }
    }
}
