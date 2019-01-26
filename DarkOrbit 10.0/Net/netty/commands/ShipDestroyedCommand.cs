using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ShipDestroyedCommand
    {
        public const short ID = 22038;

        public static byte[] write(int destroyedUserId, int explosionTypeId)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(explosionTypeId >> 5 | explosionTypeId << 27);
            param1.writeInt(destroyedUserId << 2 | destroyedUserId >> 30);
            return param1.ToByteArray();
        }
    }
}
