using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUITooltipModule
    {
        public static short ID = 18572;

        public static short STANDARD = 0;
        public static short RED = 1;

        public short var_1715 = 0;
        public ClientUITooltipTextFormatModule varC3i;
        public List<ClientUITextReplacementModule> varZa;
        public String baseKey = "";

        public ClientUITooltipModule(ClientUITooltipTextFormatModule param1, short param2, String param3,
                                 List<ClientUITextReplacementModule> param4)
        {
            this.varC3i = param1;
            this.baseKey = param3;
            this.var_1715 = param2;
            this.varZa = param4;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(31585);
            param1.writeShort(this.var_1715);
            param1.writeUTF(this.baseKey);
            param1.write(varC3i.write());
            param1.writeInt(this.varZa.Count);
            foreach(var rm in this.varZa)
            {
                param1.write(rm.write());
            }
            param1.writeShort(30300);
            return param1.Message.ToArray();
        }
    }
}
