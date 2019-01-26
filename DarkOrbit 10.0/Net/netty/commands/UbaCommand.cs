using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class UbaCommand : class_NQ
    {
        public const short ID = 14585;

        public static byte[] write(UbaG3FModule rank, Uba64iModule varA3j, UbahsModule vare3G)
        {
            var param1 = new ByteArray(ID);
            writeExtend(param1);
            param1.write(rank.write());
            param1.writeShort(-9335);
            param1.write(varA3j.write());
            param1.write(vare3G.write());
            return param1.ToByteArray();
        }
    }
}
