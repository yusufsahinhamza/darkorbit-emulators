using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUISlotBarModule
    {
        public static short ID = 30741;

        public String var_536 = "";
        public String slotBarId = "";
        public String var_2186 = "";
        public List<ClientUISlotBarItemModule> mClientUISlotBarItemModule;
        public bool visible = false;

        public ClientUISlotBarModule(String param1, String pSlotBarId, String param3,
                                 List<ClientUISlotBarItemModule> pClientUISlotBarItemModule, bool param5)
        {
            this.var_2186 = param1;
            this.slotBarId = pSlotBarId;
            this.var_536 = param3;
            this.mClientUISlotBarItemModule = pClientUISlotBarItemModule;
            this.visible = param5;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.visible);
            param1.writeUTF(this.var_2186);
            param1.writeShort(-18790);
            param1.writeUTF(this.slotBarId);
            param1.writeInt(this.mClientUISlotBarItemModule.Count);
            foreach(var im in this.mClientUISlotBarItemModule)
            {
                param1.write(im.write());
            }
            param1.writeShort(-4919);
            param1.writeUTF(this.var_536);
            return param1.Message.ToArray();
        }
    }
}
