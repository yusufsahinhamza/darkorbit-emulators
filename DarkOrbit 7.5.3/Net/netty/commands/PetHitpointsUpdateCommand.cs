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
        public const short ID = 27483;

        public static byte[] write(int hitpointsNow, int hitpointsMax, bool useRepairGear)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(hitpointsNow);
            param1.writeInt(hitpointsMax);
            param1.writeBoolean(useRepairGear);
            return param1.ToByteArray();
        }
    }
}
