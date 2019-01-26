using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class HitpointInfoCommand
    {
        public static short ID = 30056;

        public static byte[] write(int hitpoints, int hitpointsMax, int nanoHull, int nanoHullMax)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(hitpoints);
            param1.writeInt(hitpointsMax);
            param1.writeInt(nanoHull);
            param1.writeInt(nanoHullMax);
            return param1.ToByteArray();
        }
    }
}
