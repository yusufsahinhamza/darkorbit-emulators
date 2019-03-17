using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    abstract class command_NQ
    {
        public abstract byte[] write();

        protected static byte[] super(ByteArray param1)
        {
            param1.writeShort(-30700);
            param1.writeShort(12308);
            return param1.ToByteArray();
        }
    }
}
