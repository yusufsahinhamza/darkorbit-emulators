using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUITextReplacementModule
    {
        public static short ID = 1059;

        public String mReplacement = "";
        public String mWildcard = "";
        public ClientUITooltipTextFormatModule mTooltipTextFormat;

        public ClientUITextReplacementModule(String pWildCard, ClientUITooltipTextFormatModule pTooltipTextFormat,
                                         String pReplacement)
        {
            this.mWildcard = pWildCard;
            this.mReplacement = pReplacement;
            this.mTooltipTextFormat = pTooltipTextFormat;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(this.mReplacement);
            param1.write(mTooltipTextFormat.write());
            param1.writeShort(28496);
            param1.writeUTF(this.mWildcard);
            return param1.Message.ToArray();
        }
    }
}
