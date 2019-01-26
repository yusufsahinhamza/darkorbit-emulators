using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class CreatePortalCommand
    {
        public static short ID = 20288;

        public static byte[] write(int portalId, int factionId, int graphicsId, int x, int y, bool working, bool visible, List<int> varGa)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeBoolean(visible);
            param1.writeInt(factionId >> 9 | factionId << 23);
            param1.writeBoolean(working);
            param1.writeInt(portalId >> 10 | portalId << 22);
            param1.writeInt(graphicsId << 15 | graphicsId >> 17);
            param1.writeInt(x >> 12 | x << 20);
            param1.writeInt(y << 4 | y >> 28);
            param1.writeInt(varGa.Count);
            foreach(var i in varGa)
            {
                param1.writeInt(i >> 16 | i << 16);
            }
            return param1.ToByteArray();
        }
    }
}
