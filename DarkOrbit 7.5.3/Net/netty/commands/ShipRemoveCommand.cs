using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ShipRemoveCommand
    {
        public const short ID = 29006;

        public static byte[] write(int userId)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(userId);
            return param1.ToByteArray();
        }
    }
}
