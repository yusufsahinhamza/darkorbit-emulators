using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUISlotBarCategoryItemTimerStateModule
    {
        public static short ID = 686;

        public static short READY = 0;
        public static short ACTIVE = 1;
        public static short short_2168 = 2;

        public short var_1238 = 0;

        public ClientUISlotBarCategoryItemTimerStateModule(short param1)
        {
            this.var_1238 = param1;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(637);
            param1.writeShort(this.var_1238);
            return param1.Message.ToArray();
        }
    }
}
