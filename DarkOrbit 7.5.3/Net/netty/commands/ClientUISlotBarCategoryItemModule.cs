using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUISlotBarCategoryItemModule
    {
        public static short ID = 23269;

        public static short short_554 = 1;
        public static short TIMER = 3;
        public static short SELECTION = 2;
        public static short short_2465 = 0;
        public static short NUMBER = 1;
        public static short DOT = 3;
        public static short BAR = 2;
        public static short NONE = 0;

        public short counterType = 0;
        public ClientUISlotBarCategoryItemTimerModule timer;
        public ClientUISlotBarCategoryItemStatusModule status;
        public int var_586 = 0;
        public CooldownTypeModule cooldownType;
        public short actionStyle = 0;

        public ClientUISlotBarCategoryItemModule(int param1, ClientUISlotBarCategoryItemStatusModule pStatus,
                                             short pActionStyle, short pCounterType, CooldownTypeModule param4,
                                             ClientUISlotBarCategoryItemTimerModule pTimer)
        {
            this.var_586 = param1;
            this.status = pStatus;
            this.timer = pTimer;
            this.cooldownType = param4;
            this.counterType = pCounterType;
            this.actionStyle = pActionStyle;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.write(status.write());
            param1.write(timer.write());
            param1.writeShort(13478);
            param1.write(cooldownType.write());
            param1.writeInt(this.var_586 >> 3 | this.var_586 << 29);
            param1.writeShort(this.actionStyle);
            param1.writeShort(this.counterType);
            return param1.Message.ToArray();
        }
    }
}
