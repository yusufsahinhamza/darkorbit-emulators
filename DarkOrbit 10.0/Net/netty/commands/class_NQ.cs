using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_NQ
    {
        public const short Id = 6542;

        public byte[] write()
        {
            var param1 = new ByteArray(Id);
            param1.writeShort(-30700);
            param1.writeShort(12308);
            return param1.Message.ToArray();
        }

        public static byte[] writeExtend(ByteArray param1)
        {
            param1.writeShort(-30700);
            param1.writeShort(12308);
            return param1.Message.ToArray();
        }
    }
}
