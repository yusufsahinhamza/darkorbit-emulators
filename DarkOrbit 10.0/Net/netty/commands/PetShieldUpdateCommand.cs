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
        public const short ID = 8653;

        public static byte[] write(int petShieldNow, int petShieldMax)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(petShieldNow);
            param1.writeInt(petShieldMax);
            return param1.ToByteArray();
        }
    }
}
