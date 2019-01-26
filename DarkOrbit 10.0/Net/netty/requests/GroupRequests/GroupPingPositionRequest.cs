using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests.GroupRequests
{
    class GroupPingPositionRequest
    {
        public const short ID = 2656;

        public int x = 0;
        public int y = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            x = parser.readInt();
            x = x << 7 | x >> 25;
            y = parser.readInt();
            y = y >> 1 | y << 31;
        }
    }
}
