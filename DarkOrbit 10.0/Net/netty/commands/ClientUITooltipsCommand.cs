using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUITooltipsCommand
    {
        public static short ID = 11246;

        public List<ClientUITooltipModule> mTooltips;

        public ClientUITooltipsCommand(List<ClientUITooltipModule> pTooltips)
        {
            this.mTooltips = pTooltips;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(this.mTooltips.Count);
            foreach (var c in this.mTooltips)
            {
                param1.write(c.write());
            }
            param1.writeShort(-21605);
            return param1.Message.ToArray();
        }
    }
}
