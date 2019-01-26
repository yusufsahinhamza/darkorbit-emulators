using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class MineCreateCommand
    {
        public const short ID = 52;

        public static byte[] write(string hash, bool pulse, bool param3, int mineType, int y, int x)
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(hash);
            param1.writeInt(y >> 9 | y << 23);
            param1.writeInt(x >> 4 | x << 28);
            param1.writeBoolean(param3);
            param1.writeInt(mineType << 6 | mineType >> 26);
            param1.writeBoolean(pulse);
            return param1.ToByteArray();
        }
    }
}
