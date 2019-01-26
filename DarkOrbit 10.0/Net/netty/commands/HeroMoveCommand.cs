using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class HeroMoveCommand
    {
        public static short ID = 17707;

        public static byte[] write(int x, int y)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeShort(20381);
            param1.writeShort(-7620);
            param1.writeInt(x << 9 | x >> 23);
            param1.writeInt(y >> 8 | y << 24);
            return param1.ToByteArray();
        }
    }
}
