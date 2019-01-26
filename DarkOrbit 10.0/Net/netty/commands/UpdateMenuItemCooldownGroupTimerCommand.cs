using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class UpdateMenuItemCooldownGroupTimerCommand
    {
        public const short ID = 2072;

        public static byte[] write(CooldownTypeModule cooldownType, ClientUISlotBarCategoryItemTimerStateModule timerState, 
                                    long time, long maxTime)
        {
            var param1 = new ByteArray(ID);
            param1.write(timerState.write());
            param1.write(cooldownType.write());
            param1.writeDouble(time);
            param1.writeDouble(maxTime);
            param1.writeShort(-3269);
            return param1.ToByteArray();
        }
    }
}
