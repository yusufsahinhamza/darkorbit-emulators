using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class OutOfBattleStationRangeCommand
    {
        public const short ID = 15462;

        public static byte[] write(int battleStationId)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeShort(30971);
            param1.writeInt(battleStationId  >> 10 | battleStationId << 22);
            param1.writeShort(14948);
            return param1.ToByteArray();
        }
    }
}
