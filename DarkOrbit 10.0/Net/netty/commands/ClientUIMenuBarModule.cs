using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUIMenuBarModule
    {
        public static short NOT_ASSIGNED = 0;
        public static short GENERIC_FEATURE_BAR = 2;
        public static short GAME_FEATURE_BAR = 1;

        public static short ID = 21788;

        public String mPosition = "";
        public short mMenuId = 0;
        public List<ClientUIMenuBarItemModule> mMenuBarItems;
        public String var_792 = "";

        public ClientUIMenuBarModule(short param1, List<ClientUIMenuBarItemModule> param2, string param3, string param4)
        {
            this.mMenuId = param1;
            this.mMenuBarItems = param2;
            this.mPosition = param3;
            this.var_792 = param4;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(this.mMenuId);
            param1.writeUTF(this.var_792);
            param1.writeUTF(this.mPosition);
            param1.writeInt(this.mMenuBarItems.Count);
            foreach(var mbi in this.mMenuBarItems)
            {
                param1.write(mbi.write());
            }
            return param1.Message.ToArray();
        }
    }
}
