using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class MessageLocalizedWildcardCommand
    {
        public static short ID = 19731;

        public List<MessageWildcardReplacementModule> wildCardReplacements;
        public String baseKey;

        public MessageLocalizedWildcardCommand(String baseKey, List<MessageWildcardReplacementModule> wildCardReplacements)
        {
            this.baseKey = baseKey;
            this.wildCardReplacements = wildCardReplacements;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(this.baseKey);
            param1.writeInt(this.wildCardReplacements.Count);
            foreach(var _loc2_ in this.wildCardReplacements)
            {
                param1.write(_loc2_.write());
            }
            return param1.Message.ToArray();
        }
    }
}
