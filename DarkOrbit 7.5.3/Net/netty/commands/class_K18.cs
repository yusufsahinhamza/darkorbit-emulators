using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_K18
    {
        public const short ID = 702;

        public static short RED_BLINK = 5;
        public static short ACTIVE = 6;
        public static short BLUE_BLINK2 = 1;
        public static short ARROWS = 2;
        public static short BLUE_BLINK3 = 3;
        public static short BLUE_BLINK4 = 4;
        public static short BLUE_BLINK = 0;

        public short type = 0;

        public class_K18(short param1)
        {
            this.type = param1;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(-31359);
            param1.writeShort(this.type);
            return param1.Message.ToArray();
        }
    }
}
