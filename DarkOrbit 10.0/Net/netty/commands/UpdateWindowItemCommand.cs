using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class UpdateWindowItemCommand : ClientUIMenuBarItemModule
    {
        public const short ID = 14317;

        public int height = 0;     
        public Boolean maximized = false;     
        public int width = 0;   
        public String varJG = "";    
        public int y = 0;      
        public ClientUITooltipsCommand helpText;    
        public int x = 0;

        public UpdateWindowItemCommand(bool maximized, int height, bool visible, int y, int x, ClientUITooltipsCommand toolTip, string varJG, int width, ClientUITooltipsCommand helpText, string itemId) : base(visible, toolTip, itemId)
        {
            this.varJG = varJG;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.maximized = maximized;
            this.helpText = helpText;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeInt(this.height >> 14 | this.height << 18);
            param1.writeBoolean(this.maximized);
            param1.writeInt(this.width >> 2 | this.width << 30);
            param1.writeUTF(this.varJG);
            param1.writeShort(3358);
            param1.writeInt(this.y << 14 | this.y >> 18);
            param1.write(this.helpText.write());
            param1.writeInt(this.x >> 12 | this.x << 20);
            return param1.Message.ToArray();
        }
    }
}
