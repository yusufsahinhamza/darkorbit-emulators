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
        public const short ID = 19797;

        public static byte[] write(int userId)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(userId << 2 | userId >> 30);
            param1.writeShort(-9955);
            return param1.ToByteArray();
        }
    }
}
