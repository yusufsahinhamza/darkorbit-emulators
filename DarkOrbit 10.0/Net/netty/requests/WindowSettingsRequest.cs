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
        public const short ID = 30907;

        public String proActionBarPosition = "";     
        public String categoryBarPosition = "";     
        public String standartSlotBarPosition = "";     
        public String genericFeatureBarLayoutType = "";      
        public String premiumSlotBarLayoutType = "";      
        public String premiumSlotBarPosition = "";      
        public int scaleFactor = 0;      
        public String gameFeatureBarLayoutType = "";      
        public String gameFeatureBarPosition = "";      
        public String proActionBarLayoutType = "";      
        public String genericFeatureBarPosition = "";      
        public Boolean hideAllWindows = false;      
        public String standartSlotBarLayoutType = "";      
        public String unknown = "";      
        public string barStatesAsString = "";

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            this.proActionBarPosition = parser.readUTF();
            this.categoryBarPosition = parser.readUTF();
            this.standartSlotBarPosition = parser.readUTF();
            this.genericFeatureBarLayoutType = parser.readUTF();
            parser.readShort();
            this.premiumSlotBarLayoutType = parser.readUTF();
            this.premiumSlotBarPosition = parser.readUTF();
            parser.readShort();
            this.scaleFactor = parser.readInt();
            this.scaleFactor = (int)(((uint)this.scaleFactor) << 1 | ((uint)this.scaleFactor >> 31));
            this.gameFeatureBarLayoutType = parser.readUTF();
            this.gameFeatureBarPosition = parser.readUTF();
            this.proActionBarLayoutType = parser.readUTF();
            this.genericFeatureBarPosition = parser.readUTF();
            this.hideAllWindows = parser.readBoolean();
            this.standartSlotBarLayoutType = parser.readUTF();
            this.unknown = parser.readUTF();
            this.barStatesAsString = parser.readUTF();
        }
    }
}
