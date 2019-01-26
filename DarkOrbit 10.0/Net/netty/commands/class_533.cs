using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_533
    {
        public const short ID = 31992;

        public const short varTa = 2;
        public const short var03A = 1;     
        public const short varl4Q = 3;
        public const short varN2g = 0;
        public const short var21Q = 4;     
        public const short varK1M = 5;

        public short ZP;

        public class_533(short ZP)
        {
            this.ZP = ZP;
        }

        public byte[] write()
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeShort(this.ZP);
            return param1.Message.ToArray();
        }
    }
}
