using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class CpuInitializationCommand
    {
        public const short ID = 31812;

        public static byte[] write(bool varS6, bool varq37)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeBoolean(varS6);
            param1.writeBoolean(varq37);
            return param1.ToByteArray();
        }
    }
}
