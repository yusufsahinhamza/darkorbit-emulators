using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class ShipSelectRequest
    {
        public const short ID = 1666;

        public int targetId;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            targetId = param1.readInt();
        }
    }
}
