using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AbilityStopCommand
    {
        public const short ID = 2586;

        public static byte[] write(int selectedAbilityId, int activatorId, List<int> targetIds)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(selectedAbilityId >> 14 | selectedAbilityId << 18);
            param1.writeInt(targetIds.Count);
            foreach(var _loc2_ in targetIds)
            {
                param1.writeInt(_loc2_ << 6 | _loc2_ >> 26);
            }
            param1.writeInt(activatorId << 10 | activatorId >> 22);
            return param1.ToByteArray();
        }
    }
}
