using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetUIRepairButtonCommand
    {
        public const short ID = 4330;

        public static byte[] write(bool enabled, int repairCosts)
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(enabled);
            param1.writeInt(repairCosts << 10 | repairCosts >> 22);
            return param1.ToByteArray();
        }
    }
}
