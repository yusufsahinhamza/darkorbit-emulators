using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_11d
    {
        public const short ID = 14030;

        public const short const_1714 = 2;
        public const short DEFAULT = 0;
        public const short const_2330 = 1;

        public short var93y = 0;

        public class_11d(short param1)
        {
            this.var93y = param1;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(-21374);
            param1.writeShort(this.var93y);
            return param1.Message.ToArray();
        }
    }
}
