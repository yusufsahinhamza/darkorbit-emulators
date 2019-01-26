using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ShipInitializationCommandLast
    {
        public const short ID = 10810;

        public static byte[] write(int userID, String username, String shipType, int speed, int shield, int maxShield,
                                     int hp, int maxHp, int cargo, int maxCargo, int nanoHull, int maxNanoHull, int x,
                                     int y, int mapID, int factionID, int pClanID, int expansionStage, bool premium,
                                     double experience, double honor, short level, long credits, long uridium,
                                     float jackpot, int rankID, String clanTag, int rings, bool unknown2,
                                     bool cloaked, bool unknown3, List<VisualModifierCommandLast> pModifiers)
        {



            var param1 = new ByteArray(ID);
            param1.writeBoolean(unknown3);
            param1.writeInt(maxNanoHull << 6 | maxNanoHull >> 26);
            param1.writeInt(pClanID << 3 | pClanID >> 29);
            param1.writeInt(hp << 15 | hp >> 17);
            param1.writeFloat(jackpot);
            param1.writeInt(mapID >> 13 | mapID << 19);
            param1.writeInt(rings << 7 | rings >> 25);
            param1.writeInt(cargo >> 8 | cargo << 24);
            param1.writeInt(maxCargo << 13 | maxCargo >> 19);
            param1.writeInt(rankID >> 3 | rankID << 29);
            param1.writeBoolean(unknown2);
            param1.writeInt(pModifiers.Count);
            foreach (var modifier in pModifiers)
            {
                param1.write(modifier.write());
            }
            param1.writeInt(x << 13 | x >> 19);
            param1.writeBoolean(cloaked);
            param1.writeDouble(credits);
            param1.writeUTF(shipType);
            param1.writeInt(maxShield >> 5 | maxShield << 27);
            param1.writeInt(userID << 2 | userID >> 30);
            param1.writeDouble(honor);
            param1.writeInt(y << 3 | y >> 29);
            param1.writeUTF(username);
            param1.writeInt(nanoHull >> 13 | nanoHull << 19);
            param1.writeInt(maxHp >> 9 | maxHp << 23);
            param1.writeUTF(clanTag);
            param1.writeDouble(experience);
            param1.writeInt(speed << 14 | speed >> 18);
            param1.writeInt(level << 6 | level >> 26);
            param1.writeInt(shield >> 16 | shield << 16);
            param1.writeBoolean(premium);
            param1.writeDouble(uridium);
            param1.writeInt(expansionStage >> 3 | expansionStage << 29);
            param1.writeInt(factionID >> 13 | factionID << 19);
            return param1.ToByteArray();
        }
    }
}
