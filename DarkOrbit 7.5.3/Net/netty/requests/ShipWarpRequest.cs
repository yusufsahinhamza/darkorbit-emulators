using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class ShipWarpRequest
    {
        public const short ID = 1450;

        public int shipId;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            shipId = param1.readInt();
        }
    }
}
