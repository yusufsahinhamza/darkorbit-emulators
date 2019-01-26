using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AttributeHitpointUpdateCommand
    {
        public static short ID = 8638;

        public static byte[] write(int hitpointsNow, int hitpointsMax)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(hitpointsNow);
            param1.writeInt(hitpointsMax);
            return param1.ToByteArray();
        }
    }
}
