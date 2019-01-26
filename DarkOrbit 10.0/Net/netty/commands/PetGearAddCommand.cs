using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetGearAddCommand
    {
        public const short ID = 28351;

        public static byte[] write(PetGearTypeModule gearType, int level, int amount, bool enabled)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(amount >> 10 | amount << 22);
            param1.writeShort(17438);
            param1.writeBoolean(enabled);
            param1.writeInt(level >> 2 | level << 30);
            param1.write(gearType.write());
            return param1.ToByteArray();
        }
    }
}
