using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class RemovePortalCommand
    {
        public const short ID = 31113;

        public static byte[] write(int portalId)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(portalId << 10 | portalId >> 22);
            param1.writeShort(-12062);
            return param1.ToByteArray();
        }
    }
}
