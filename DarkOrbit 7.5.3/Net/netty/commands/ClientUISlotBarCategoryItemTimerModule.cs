using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUISlotBarCategoryItemTimerModule
    {
        public static short ID = 31208;

        public ClientUISlotBarCategoryItemTimerStateModule timerState;
        public String var_1474 = "";
        public bool activatable = false;
        public double time = 0;
        public double maxTime = 0;

        public ClientUISlotBarCategoryItemTimerModule(double pTime, ClientUISlotBarCategoryItemTimerStateModule pTimerState,
                                                  double pMaxTime, String param1, bool pActivatable)
        {
            this.var_1474 = param1;
            this.timerState = pTimerState;
            this.time = pTime;
            this.maxTime = pMaxTime;
            this.activatable = pActivatable;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeDouble(this.time);
            param1.writeUTF(this.var_1474);
            param1.writeBoolean(this.activatable);
            param1.writeDouble(this.maxTime);
            param1.write(timerState.write());
            return param1.Message.ToArray();
        }
    }
}
