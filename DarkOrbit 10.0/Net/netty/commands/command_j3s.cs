using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    abstract class command_j3s
    {
        public abstract byte[] write();

        protected static byte[] super(ByteArray param1)
        {
            param1.writeShort(18611);
            return param1.ToByteArray();
        }
    }
}
