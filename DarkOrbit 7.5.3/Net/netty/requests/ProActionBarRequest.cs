using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class ProActionBarRequest
    {
        public const short ID = 31697;

        public bool opened;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            parser.readShort();
            opened = parser.readBoolean();
        }
    }
}
