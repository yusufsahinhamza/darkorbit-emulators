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
        public const short ID = 19586;

        public static byte[] write(int userId, int shipType, int shield, int shieldMax, int hitpoints, int hitpointsMax, int nanoHull, int maxNanoHull, bool shieldSkill)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(userId);
            param1.writeInt(shipType);
            param1.writeInt(shield);
            param1.writeInt(shieldMax);
            param1.writeInt(hitpoints);
            param1.writeInt(hitpointsMax);
            param1.writeInt(nanoHull);
            param1.writeInt(maxNanoHull);
            param1.writeBoolean(shieldSkill);
            return param1.ToByteArray();
        }
    }
}
