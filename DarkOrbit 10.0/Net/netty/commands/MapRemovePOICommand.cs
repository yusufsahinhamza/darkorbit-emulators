using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class MapRemovePOICommand
    {
        public const short ID = 27912;

        public static byte[] write(string poiId)
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(16871);
            param1.writeUTF(poiId);
            return param1.ToByteArray();
        }
    }
}
