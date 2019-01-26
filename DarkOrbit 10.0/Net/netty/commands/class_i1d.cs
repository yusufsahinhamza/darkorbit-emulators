using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_i1d
    {
        public const short ID = 11298;

        public class_O4f vari2P;      
        public class_y3i varC2P;

        public class_i1d(class_O4f vari2P, class_y3i varC2P)
        {
            this.vari2P = vari2P;
            this.varC2P = varC2P;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(-20219);
            param1.write(vari2P.write());
            param1.write(varC2P.write());
            param1.writeShort(28155);
            return param1.Message.ToArray();
        }
    }
}
