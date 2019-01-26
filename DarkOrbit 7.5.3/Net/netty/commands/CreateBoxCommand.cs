using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class CreateBoxCommand
    {
        public const short ID = 18425;

        public static byte[] write(string type, string hash, int y, int x)
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(hash);
            param1.writeInt(y >> 9 | y << 23);
            param1.writeInt(x >> 4 | x << 28);
            param1.writeUTF(type);
            param1.writeShort(21295);
            return param1.ToByteArray();
        }
    }
}
