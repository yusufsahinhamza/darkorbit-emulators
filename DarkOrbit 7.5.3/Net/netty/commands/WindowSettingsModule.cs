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
        public const short ID = 12419;

        public Boolean notSet = false;
      
        public int clientResolutionId = 0;
      
        public String windowSettings = "";
      
        public String resizableWindows = "";
      
        public int minmapScale = 0;
      
        public String mainmenuPosition = "";
      
        public String barStatus = "";
       
        public String slotmenuPosition = "";
      
        public String slotMenuOrder = "";
      
        public String slotmenuPremiumPosition = "";
      
        public String slotMenuPremiumOrder = "";

        public WindowSettingsModule(Boolean notSet, int clientResolutionId, String windowSettings, String resizableWindows, int minmapScale,
            String mainmenuPosition, String barStatus, String slotmenuPosition, String slotMenuOrder, String slotmenuPremiumPosition, String slotMenuPremiumOrder)
        {
            this.notSet = notSet;
            this.clientResolutionId = clientResolutionId;
            this.windowSettings = windowSettings;
            this.resizableWindows = resizableWindows;
            this.minmapScale = minmapScale;
            this.mainmenuPosition = mainmenuPosition;
            this.barStatus = barStatus;
            this.slotmenuPosition = slotmenuPosition;
            this.slotMenuOrder = slotMenuOrder;
            this.slotmenuPremiumPosition = slotmenuPremiumPosition;
            this.slotMenuPremiumOrder = slotMenuPremiumOrder;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.notSet);
            param1.writeInt(this.clientResolutionId);
            param1.writeUTF(this.windowSettings);
            param1.writeUTF(this.resizableWindows);
            param1.writeInt(this.minmapScale);
            param1.writeUTF(this.mainmenuPosition);
            param1.writeUTF(this.barStatus);
            param1.writeUTF(this.slotmenuPosition);
            param1.writeUTF(this.slotMenuOrder);
            param1.writeUTF(this.slotmenuPremiumPosition);
            param1.writeUTF(this.slotMenuPremiumOrder);
            return param1.Message.ToArray();
        }
    }
}
