using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AttributeShieldUpdateCommand
    {
        public static short ID = 4550;

        public static byte[] write(int shieldNow, int shieldMax)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(shieldNow >> 12 | shieldNow << 20);
            param1.writeInt(shieldMax >> 7 | shieldMax << 25);
            return param1.ToByteArray();
        }
    }
}
