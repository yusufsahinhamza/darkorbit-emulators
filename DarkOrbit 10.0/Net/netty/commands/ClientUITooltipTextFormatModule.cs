using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUITooltipTextFormatModule
    {
        public static short const_1089 = 3;
        public static short const_234 = 7;
        public static short const_1964 = 6;
        public static short LOCALIZED = 5;
        public static short PLAIN = 0;
        public static short const_2514 = 1;
        public static short const_2280 = 2;
        public static short const_2046 = 4;

        public static short ID = 24892;
        public short var_1413 = 0;

        public ClientUITooltipTextFormatModule(short param1)
        {
            this.var_1413 = param1;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(this.var_1413);
            param1.writeShort(26386);
            return param1.Message.ToArray();
        }
    }
}
