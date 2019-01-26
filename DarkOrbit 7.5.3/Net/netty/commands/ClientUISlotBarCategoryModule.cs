using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUISlotBarCategoryModule
    {
        public static short ID = 23559;

        public List<ClientUISlotBarCategoryItemModule> mClientUISlotBarCategoryItemModuleVector;
        public String tooltip = "";

        public ClientUISlotBarCategoryModule(String param1, List<ClientUISlotBarCategoryItemModule> param2)
        {
            this.tooltip = param1;
            this.mClientUISlotBarCategoryItemModuleVector = param2;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(this.tooltip);
            param1.writeInt(this.mClientUISlotBarCategoryItemModuleVector.Count);
            foreach (var c in this.mClientUISlotBarCategoryItemModuleVector)
            {
                param1.write(c.write());
            }
            return param1.Message.ToArray();
        }
    }
}
