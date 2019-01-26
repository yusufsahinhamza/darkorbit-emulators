using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_h2P
    {
        public const short ID = 30023;

        public const short FEATURE_MENU = 1;
        public const short ITEMS_CONTROL = 0;
        public const short WINDOW = 2;

        public short vare1d = 0;

        public class_h2P(short param1)
        {
            this.vare1d = param1;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(this.vare1d);
            return param1.Message.ToArray();
        }
    }
}
