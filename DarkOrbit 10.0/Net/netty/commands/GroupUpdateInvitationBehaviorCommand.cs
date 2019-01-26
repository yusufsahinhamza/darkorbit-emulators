using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupUpdateInvitationBehaviorCommand
    {
        public const short ID = 19280;

        public const short everyone = 1;     
        public const short leader = 0;

        public static byte[] write(short mode)
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(22289);
            param1.writeShort(mode);
            return param1.ToByteArray();
        }
    }
}
