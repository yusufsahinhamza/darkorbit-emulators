using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests.GroupRequests
{
    class GroupChangeLeaderRequest
    {
        public const short ID = 540;

        public int userId = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            userId = parser.readInt();
            userId = userId >> 4 | userId << 28;
        }
    }
}
