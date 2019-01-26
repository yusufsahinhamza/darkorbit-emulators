using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class SelectMenuBarItemRequest
    {
        public const short ID = 18889;

        public static short const_2391 = 0;
        public static short const_2225 = 1;
        public static short SELECT = 0;
        public static short ACTIVATE = 1;

        public String itemId = "";
        public short varat = 0;
        public short varH4g = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            varat = parser.readShort();
            varH4g = parser.readShort();
            itemId = parser.readUTF();
            parser.readShort();
        }
    }
}
