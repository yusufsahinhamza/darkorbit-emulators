using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class DisposeBoxCommand
    {
        public const short ID = 25477;

        public static byte[] write(string hash, bool param2)
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(hash);
            param1.writeBoolean(param2);
            return param1.ToByteArray();
        }
    }
}
