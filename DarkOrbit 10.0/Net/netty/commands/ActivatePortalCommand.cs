using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ActivatePortalCommand
    {
        public const short ID = 2310;

        public static byte[] write(int mapId, int portalId)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(mapId >> 3 | mapId << 29);
            param1.writeInt(portalId << 15 | portalId >> 17);
            param1.writeShort(20220);
            return param1.ToByteArray();
        }
    }
}
