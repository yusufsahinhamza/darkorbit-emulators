using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class ReadyRequest
    {
        public const short ID = 18168;

        public short readyType;
        public const short MAP_LOADED = 0;
        public const short HERO_LOADED = 1;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort();
            readyType = parser.readShort();
            parser.readShort();
        }
    }
}
