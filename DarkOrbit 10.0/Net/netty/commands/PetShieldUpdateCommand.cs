using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetShieldUpdateCommand
    {
        public const short ID = 24674;

        public static byte[] write(int petShieldNow, int petShieldMax)
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(-13093);
            param1.writeInt(petShieldNow << 2 | petShieldNow >> 30);
            param1.writeInt(petShieldMax >> 1 | petShieldMax << 31);
            return param1.ToByteArray();
        }
    }
}
