using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ShipSettingsCommand
    {
        public const short ID = 12067;

        public static byte[] write(string quickbarSlots, string quickbarSlotsPremium, int selectedLaser, int selectedRocket, int selectedHellstormRocket)
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(quickbarSlots);
            param1.writeUTF(quickbarSlotsPremium);
            param1.writeInt(selectedLaser);
            param1.writeInt(selectedRocket);
            param1.writeInt(selectedHellstormRocket);
            return param1.ToByteArray();
        }
    }
}
