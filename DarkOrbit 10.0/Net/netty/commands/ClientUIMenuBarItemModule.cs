using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    abstract class ClientUIMenuBarItemModule
    {
        public abstract byte[] write();

        public String itemId = "";
        public bool visible = false;
        public ClientUITooltipsCommand toolTip;

        public ClientUIMenuBarItemModule(bool visible, ClientUITooltipsCommand toolTip, String itemId)
        {
            this.visible = visible;
            this.toolTip = toolTip;
            this.itemId = itemId;
        }

        protected byte[] super(ByteArray param1)
        {
            param1.writeShort(11362);
            param1.write(this.toolTip.write());
            param1.writeUTF(this.itemId);
            param1.writeBoolean(this.visible);
            return param1.ToByteArray();
        }
    }
}
