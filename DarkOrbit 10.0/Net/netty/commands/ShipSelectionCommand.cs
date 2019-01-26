using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ShipSelectionCommand
    {
        public const short ID = 11468;

        public static byte[] write(int userId, int shipType, int shield, int shieldMax, int hitpoints, int hitpointsMax, int nanoHull, int maxNanoHull, bool shieldSkill)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(maxNanoHull >> 10 | maxNanoHull << 22);
            param1.writeInt(shipType << 7 | shipType >> 25);
            param1.writeInt(hitpoints << 8 | hitpoints >> 24);
            param1.writeInt(shield >> 12 | shield << 20);
            param1.writeInt(shieldMax >> 9 | shieldMax << 23);
            param1.writeInt(hitpointsMax << 13 | hitpointsMax >> 19);
            param1.writeInt(userId << 9 | userId >> 23);
            param1.writeInt(nanoHull >> 6 | nanoHull << 26);
            param1.writeBoolean(shieldSkill);
            return param1.ToByteArray();
        }
    }
}
