using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AbilityEffectDeActivationCommand
    {
        public const short ID = 19907;

        public static byte[] write(int selectedAbilityId, int activatorId, List<int> targetIds)
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(10776);
            param1.writeInt(targetIds.Count);
            foreach(var _loc2_ in targetIds)
            {
                param1.writeInt(_loc2_ >> 5 | _loc2_ << 27);
            }
            param1.writeInt(selectedAbilityId >> 1 | selectedAbilityId << 31);
            param1.writeInt(activatorId >> 12 | activatorId << 20);
            return param1.ToByteArray();
        }
    }
}
