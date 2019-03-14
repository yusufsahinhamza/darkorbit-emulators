using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests.GroupRequests
{
    class GroupPingPlayerRequest
    {
        public const short ID = 32380;

        public int userId = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            userId = parser.readInt();
            userId = (int)(((uint)userId) << 12 | ((uint)userId >> 20));
        }
    }
}
