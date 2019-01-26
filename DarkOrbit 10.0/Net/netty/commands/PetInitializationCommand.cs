using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetInitializationCommand
    {
        public const short ID = 9174;

        public static byte[] write(bool hasPet, bool hasFuel, bool petIsAlive)
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(hasPet);
            param1.writeBoolean(hasFuel);
            param1.writeBoolean(petIsAlive);
            return param1.ToByteArray();
        }
    }
}
