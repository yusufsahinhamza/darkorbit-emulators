using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class tetetestcommand
    {
        public const short ID = 32054;

        public static byte[] write(int battleStationId, int minutes)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(minutes >> 9 | minutes << 23);
            param1.writeInt(battleStationId << 7 | battleStationId >> 25);
            return param1.ToByteArray();
        }
    }
}
