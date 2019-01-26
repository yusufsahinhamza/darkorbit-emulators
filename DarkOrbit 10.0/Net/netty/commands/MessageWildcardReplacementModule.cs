using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class MessageWildcardReplacementModule
    {
        public const short ID = 1059;

        public ClientUITooltipTextFormatModule var_1507;
        public String wildcard;
        public String replacement;

        public MessageWildcardReplacementModule(String param1, String param2, ClientUITooltipTextFormatModule param3)
        {
            this.wildcard = param1;
            this.replacement = param2;
            this.var_1507 = param3;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(this.replacement);
            param1.write(var_1507.write());
            param1.writeShort(28496);
            param1.writeUTF(this.wildcard);
            return param1.Message.ToArray();
        }
    }
}
