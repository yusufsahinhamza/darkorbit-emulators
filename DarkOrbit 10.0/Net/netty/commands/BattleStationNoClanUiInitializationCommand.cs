using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BattleStationNoClanUiInitializationCommand
    {
        public static short ID = 3062;

        public static byte[] write(int mapAssetId)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(mapAssetId >> 10 | mapAssetId << 22);
            param1.writeShort(-18420);
            return param1.ToByteArray();
        }
    }
}
