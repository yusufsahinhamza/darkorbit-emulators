using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupInviteCommand
    {
        public const short ID = 596;

        public static byte[] write(int userId, string name, GroupPlayerShipModule ship, int invitedUserId, string invitedName, GroupPlayerShipModule invitedShip)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.write(ship.write());
            param1.writeInt(invitedUserId >> 9 | invitedUserId << 23);
            param1.writeInt(userId >> 11 | userId << 21);
            param1.write(invitedShip.write());
            param1.writeUTF(name);
            param1.writeUTF(invitedName);
            param1.writeShort(-2976);
            param1.writeShort(-27904);
            return param1.ToByteArray();
        }
    }
}
