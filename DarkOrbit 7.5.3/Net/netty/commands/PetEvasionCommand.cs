using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetEvasionCommand
    {
        public const short ID = 4043;

        public static byte[] write(int petId, bool evasionActive)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(petId);
            param1.writeBoolean(evasionActive);
            return param1.ToByteArray();
        }
    }
}
