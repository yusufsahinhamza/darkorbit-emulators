using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ShipInitializationCommand
    {
        public const short ID = 26642;

        public static byte[] write(int userId, String userName, int shipType, int speed, int shield, int shieldMax,
            int hitPoints, int hitMax, int cargoSpace, int cargoSpaceMax, int nanoHull, int maxNanoHull, int x, int y,
            int mapId, int factionId, int clanId, int laserBatteriesMax, int rocketsMax, int expansionStage, Boolean premium,
            long ep, long honourPoints, int level, long credits, long uridium, float jackpot, int dailyRank, String clanTag,
            int galaxyGatesDone, Boolean useSystemFont, Boolean cloaked, List<VisualModifierCommand> modifier)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(userId);
            param1.writeUTF(userName);
            param1.writeInt(shipType);
            param1.writeInt(speed);
            param1.writeInt(shield);
            param1.writeInt(shieldMax);
            param1.writeInt(hitPoints);
            param1.writeInt(hitMax);
            param1.writeInt(cargoSpace);
            param1.writeInt(cargoSpaceMax);
            param1.writeInt(nanoHull);
            param1.writeInt(maxNanoHull);
            param1.writeInt(x);
            param1.writeInt(y);
            param1.writeInt(mapId);
            param1.writeInt(factionId);
            param1.writeInt(clanId);
            param1.writeInt(laserBatteriesMax);
            param1.writeInt(rocketsMax);
            param1.writeInt(expansionStage);
            param1.writeBoolean(premium);
            param1.writeDouble(ep);
            param1.writeDouble(honourPoints);
            param1.writeInt(level);
            param1.writeDouble(credits);
            param1.writeDouble(uridium);
            param1.writeFloat(jackpot);
            param1.writeInt(dailyRank);
            param1.writeUTF(clanTag);
            param1.writeInt(galaxyGatesDone);
            param1.writeBoolean(useSystemFont);
            param1.writeBoolean(cloaked);
            param1.writeInt(modifier.Count);
            foreach(var m in modifier)
            {
                param1.write(m.write());
            }
            return param1.ToByteArray();
        }
    }
}
