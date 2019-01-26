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
        public const short ID = 11189;

        public static byte[] write(int destroyedUserId, int explosionTypeId)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(destroyedUserId);
            param1.writeInt(explosionTypeId);
            return param1.ToByteArray();
        }
    }
}
