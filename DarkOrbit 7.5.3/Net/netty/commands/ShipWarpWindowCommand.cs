using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class ShipWarpWindowCommand
    {
        public const short ID = 32348;

        public static byte[] write(int jumpVoucherCount, int uridium, bool isNearSpacestation, List<ShipWarpModule> ships)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(jumpVoucherCount);
            param1.writeInt(uridium);
            param1.writeBoolean(isNearSpacestation);
            param1.writeInt(ships.Count);
            foreach(var _loc2_ in ships)
            {
                param1.write(_loc2_.write());
            }
            return param1.ToByteArray();
        }
    }
}
