using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class command_e4W
    {
        public const short ID = 7725;

        public int varad, varI3j;
        public string vard1r, var12h;
        public class_oS varS1l;
        public class_s16 varY45;

        public command_e4W(int param1, int param2, string param3, string param4, class_oS param5, class_s16 param6)
        {
            varad = param1;
            varI3j = param2;
            vard1r = param3;
            var12h = param4;
            varS1l = param5;
            varY45 = param6;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(-30700);
            param1.writeShort(12308);
            param1.writeInt(varad >> 16 | varad << 16);
            param1.write(varY45.write());
            param1.writeInt(varI3j << 13 | varI3j >> 19);
            param1.writeUTF(vard1r);
            param1.writeShort(25583);
            param1.write(varS1l.write());
            param1.writeUTF(var12h);
            return param1.ToByteArray();
        }
    }
}
