using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class command_i3O
    {
        //public const short ID = 32339;

        public static byte[] super(ByteArray param1)
        {
            param1.writeShort(29809);
            param1.writeShort(15227);
            return param1.ToByteArray();
        }
    }
}
