using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetBlockUICommand
    {
        public const short ID = 7348;

        public static byte[] write(bool blocked)
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(blocked);
            return param1.ToByteArray();
        }
    }
}
