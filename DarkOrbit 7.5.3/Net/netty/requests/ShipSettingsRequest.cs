using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class ShipSettingsRequest
    {
        public const short ID = 1336;

        public String quickbarSlots = "";

        public String quickbarSlotsPremium = "";

        public int selectedLaser = 0;

        public int selectedRocket = 0;

        public int selectedHellstormRocket = 0;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            this.quickbarSlots = param1.readUTF();
            this.quickbarSlotsPremium = param1.readUTF();
            this.selectedLaser = param1.readInt();
            this.selectedRocket = param1.readInt();
            this.selectedHellstormRocket = param1.readInt();
        }
    }
}
