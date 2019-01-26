using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class WindowSettingsModule
    {
        public const short ID = 1110;

        public bool HideAllWindows = false;
        public int Scale = 0;
        public String BarState = "";

        public WindowSettingsModule(int param1, String param2, bool param3)
        {
            this.Scale = param1;
            this.BarState = param2;
            this.HideAllWindows = param3;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(this.BarState);
            param1.writeInt(this.Scale << 13 | this.Scale >> 19);
            param1.writeBoolean(this.HideAllWindows);
            param1.writeShort(-27662);
            param1.writeShort(15301);
            return param1.Message.ToArray();
        }
    }
}
