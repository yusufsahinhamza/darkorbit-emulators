using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetHitpointsUpdateCommand
    {
        public const short ID = 29823;

        public static byte[] write(int hitpointsNow, int hitpointsMax, bool useRepairGear)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(hitpointsMax >> 3 | hitpointsMax << 29);
            param1.writeShort(27532);
            param1.writeBoolean(useRepairGear);
            param1.writeInt(hitpointsNow << 7 | hitpointsNow >> 25);
            return param1.ToByteArray();
        }
    }
}
