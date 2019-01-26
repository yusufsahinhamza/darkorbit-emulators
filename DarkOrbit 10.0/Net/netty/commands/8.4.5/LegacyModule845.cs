using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class LegacyModule845
    {
        public const short ID = 31017;
        
        public static byte[] write(string message)
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(message);
            param1.writeShort(-28556);
            return param1.ToByteArray();
        }
    }
}
