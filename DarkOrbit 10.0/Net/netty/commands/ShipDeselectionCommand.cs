using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ShipDeselectionCommand
    {
        public const short ID = 25099;

        public static byte[] write()
        {
            ByteArray param1 = new ByteArray(ID);
            return param1.ToByteArray();
        }
    }
}
