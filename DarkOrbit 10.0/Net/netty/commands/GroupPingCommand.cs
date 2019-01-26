using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupPingCommand
    {
        public const short ID = 20661;

        public static byte[] write(int x, int y)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(y << 1 | y >> 31);
            param1.writeShort(5139);
            param1.writeInt(x << 5 | x >> 27);
            return param1.ToByteArray();
        }
    }
}
