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
        public static short ID = 21243;

        public static byte[] write(int shieldNow, int shieldMax)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(shieldNow);
            param1.writeInt(shieldMax);
            return param1.ToByteArray();
        }
    }
}
