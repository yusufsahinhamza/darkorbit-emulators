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
        public static short ID = 24000;

        public static byte[] write(int x, int y)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(x);
            param1.writeInt(y);
            return param1.ToByteArray();
        }
    }
}
