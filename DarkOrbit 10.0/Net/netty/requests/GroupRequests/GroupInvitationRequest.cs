using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests.GroupRequests
{
    class GroupInvitationRequest
    {
        public const short ID = 13359;

        public string name = "";

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            this.name = parser.readUTF();
            parser.readShort();
        }
    }
}
