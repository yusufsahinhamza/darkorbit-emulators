using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class WindowSettingsRequest
    {
        public const short ID = 19710;

        public int clientResolutionId = 0;
      
        public String windowSettings = "";
      
        public String resizableWindows = "";
      
        public int minimapScale = 0;
      
        public String mainmenuPosition = "";
       
        public String slotmenuPosition = "";
      
        public String slotMenuOrder = "";
      
        public String slotmenuPremiumPosition = "";
      
        public String slotMenuPremiumOrder = "";
      
        public String barStatus = "";

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            this.clientResolutionId = param1.readShort();
            this.windowSettings = param1.readUTF();
            this.resizableWindows = param1.readUTF();
            this.minimapScale = param1.readInt();
            this.mainmenuPosition = param1.readUTF();
            this.slotmenuPosition = param1.readUTF();
            this.slotMenuOrder = param1.readUTF();
            this.slotmenuPremiumPosition = param1.readUTF();
            this.slotMenuPremiumOrder = param1.readUTF();
            this.barStatus = param1.readUTF();
        }
    }
}
