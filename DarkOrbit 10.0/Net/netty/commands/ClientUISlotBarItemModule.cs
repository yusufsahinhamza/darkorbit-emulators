using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUISlotBarItemModule
    {
        public static short ID = 14620;

        public String itemId = "";
        public int slotId = 0;

        public ClientUISlotBarItemModule(String itemId, int pSlotId)
        {
            this.itemId = itemId;
            this.slotId = pSlotId;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(this.itemId);
            param1.writeInt(this.slotId << 5 | this.slotId >> 27);
            return param1.Message.ToArray();
        }
    }
}
