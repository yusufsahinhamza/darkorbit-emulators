using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests.GroupRequests
{
    class GroupAcceptInvitationRequest
    {
        public const short ID = 11175;

        public int userId = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            this.userId = parser.readInt();
            this.userId = this.userId >> 14 | this.userId << 18;
            parser.readShort();
        }
    }
}
