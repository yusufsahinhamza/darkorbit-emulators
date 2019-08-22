using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class SendWindowUpdateRequest
    {
        public const short ID = 7606;

        public int y = 0;   
        public int width = 0;     
        public String itemId = "";     
        public int height = 0;     
        public int x = 0;    
        public Boolean maximized = false;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            this.y = parser.readInt();
            this.y = (int)(((uint)this.y >> 11) | ((uint)this.y << 21));
            this.width = parser.readInt();
            this.width = (int)(((uint)this.width >> 8) | ((uint)this.width << 24));
            this.itemId = parser.readUTF();
            this.height = parser.readInt();
            this.height = (int)(((uint)this.height << 8) | ((uint)this.height >> 24));
            this.x = parser.readInt();
            this.x = (int)(((uint)this.x >> 11) | ((uint)this.x << 21));
            this.maximized = parser.readBoolean();
        }
    }
}
