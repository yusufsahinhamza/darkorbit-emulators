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
        public static short ID = 11751;

        public List<MessageWildcardReplacementModule> mWildCardReplacements;
        public String mBaseKey;
        public ClientUITooltipTextFormatModule var_3317;

        public MessageLocalizedWildcardCommand(String pBaseKey, ClientUITooltipTextFormatModule param2,
                                           List<MessageWildcardReplacementModule> pWildCardReplacements)
        {
            this.mBaseKey = pBaseKey;
            this.var_3317 = param2;
            this.mWildCardReplacements = pWildCardReplacements;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(-27064);
            param1.writeShort(-14597);
            param1.writeInt(this.mWildCardReplacements.Count);
            foreach(var rm in this.mWildCardReplacements)
            {
                param1.write(rm.write());
            }
            param1.writeUTF(this.mBaseKey);
            param1.write(var_3317.write());
            return param1.Message.ToArray();
        }
    }
}
