using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_s16
    {
        public const short ID = 26667;

        public static short varC3p = 1;
        public static short varDH = 0;

        public short varx6 = 0;
        public int varA1B = 0;

        public class_s16(int varA1B, short varx6)
        {
            this.varA1B = varA1B;
            this.varx6 = varx6;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(this.varx6);
            param1.writeInt(this.varA1B << 16 | this.varA1B >> 16);
            return param1.Message.ToArray();
        }
    }
}
