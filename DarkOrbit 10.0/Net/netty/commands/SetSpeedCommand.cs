using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class SetSpeedCommand
    {
        public static short ID = 4082;

        public static byte[] write(int var93, int varQ1E)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(varQ1E << 2 | varQ1E >> 30);
            param1.writeInt(var93 << 8 | var93 >> 24);
            param1.writeShort(-5122);
            return param1.ToByteArray();
        }
    }
}
