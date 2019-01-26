using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class ShipWarpCompletedCommand
    {
        public const short ID = 5630;

        public static byte[] write()
        {
            var param1 = new ByteArray(ID);
            return param1.ToByteArray();
        }
    }
}
