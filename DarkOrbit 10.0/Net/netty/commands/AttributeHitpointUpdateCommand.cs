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
        public static short ID = 15997;

        public static byte[] write(int vardV, int varw1T, int vark3E, int varsd)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(vardV >> 3 | vardV << 29);
            param1.writeInt(varw1T << 6 | varw1T >> 26);
            param1.writeInt(vark3E >> 8 | vark3E << 24);
            param1.writeInt(varsd << 6 | varsd >> 26);
            return param1.ToByteArray();
        }
    }
}
