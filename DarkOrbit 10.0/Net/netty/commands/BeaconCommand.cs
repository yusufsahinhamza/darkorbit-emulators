using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BeaconCommand
    {
        public const short ID = 10084;

        public static byte[] write(int var01e, int varu1C, int varc4N, int varFr, bool inDemiZone, bool repairBot, bool varpc,
                         String repairBotId, bool inRadiationZone)
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(varpc);
            param1.writeInt(varFr >> 4 | varFr << 28);
            param1.writeUTF(repairBotId);
            param1.writeBoolean(inDemiZone);
            param1.writeBoolean(inRadiationZone);
            param1.writeInt(varu1C >> 6 | varu1C << 26);
            param1.writeBoolean(repairBot);
            param1.writeInt(varc4N >> 7 | varc4N << 25);
            param1.writeShort(16384);
            param1.writeInt(var01e << 9 | var01e >> 23);
            return param1.ToByteArray();
        }
    }
}
