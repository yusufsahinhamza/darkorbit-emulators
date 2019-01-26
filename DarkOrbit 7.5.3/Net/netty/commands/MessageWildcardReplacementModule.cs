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
        public const short ID = 32205;

        public String wildcard;
        public String replacement;

        public MessageWildcardReplacementModule(String wildcard, String replacement)
        {
            this.wildcard = wildcard;
            this.replacement = replacement;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(this.wildcard);
            param1.writeUTF(this.replacement);
            return param1.Message.ToArray();
        }
    }
}
