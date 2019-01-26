using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUIMenuBarItemModule
    {
        public static short ID = 5748;

        public String itemId = "";
        public bool visible = false;
        public ClientUITooltipsCommand toolTip;

        public ClientUIMenuBarItemModule(bool pVisible, ClientUITooltipsCommand pTooltipCommand, String pItemId)
        {
            this.visible = pVisible;
            this.toolTip = pTooltipCommand;
            this.itemId = pItemId;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(11362);
            param1.write(toolTip.write());
            param1.writeUTF(this.itemId);
            param1.writeBoolean(this.visible);
            return param1.Message.ToArray();
        }
    }
}
