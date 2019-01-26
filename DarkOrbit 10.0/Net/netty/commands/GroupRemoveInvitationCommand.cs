using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupRemoveInvitationCommand
    {
        public const short ID = 2978;

        public const short REJECT = 3;
        public const short REVOKE = 2;
        public const short NONE = 0;
        public const short varE4J = 4;     
        public const short TIMEOUT = 1;

        public static byte[] write(int inviterId, int invitedId, short reason)
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(reason);
            param1.writeInt(invitedId << 1 | invitedId >> 31);
            param1.writeShort(-24382);
            param1.writeInt(inviterId >> 14 | inviterId << 18);
            return param1.ToByteArray();
        }
    }
}
