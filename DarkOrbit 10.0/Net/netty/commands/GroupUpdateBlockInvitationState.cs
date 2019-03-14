using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupUpdateBlockInvitationState //: command_i3O
    {
        public const short ID = 21555;

        public static byte[] write(bool active)
        {
            ByteArray param1 = new ByteArray(ID);
            //super(param1);
            param1.writeShort(29809);
            param1.writeShort(15227);
            param1.writeBoolean(active);
            return param1.ToByteArray();
        }
    }
}
