using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests.GroupRequests
{
    class GroupRevokeInvitationRequest
    {
        public const short ID = 10196;

        public int userId = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            userId = parser.readInt();
            userId = userId >> 15 | userId << 17;
        }
    }
}
