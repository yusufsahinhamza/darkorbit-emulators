using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class LegacyModule
    {
        public const short ID = 4224;
        
        public static byte[] write(string message)
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(message);
            return param1.ToByteArray();
        }
    }
}
