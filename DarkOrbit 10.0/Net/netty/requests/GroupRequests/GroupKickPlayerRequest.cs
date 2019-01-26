using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests.GroupRequests
{
    class GroupKickPlayerRequest
    {
        public const short ID = 29161;

        public int userId = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            userId = parser.readInt();
            userId = (int)(((uint)userId << 1) | ((uint)userId >> 31));
        }
    }
}
