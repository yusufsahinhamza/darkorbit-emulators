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
        public const short ID = 7511;

        public static byte[] write(int userID, String username, String shipType, int speed, int shield, int maxShield,
                                     int hp, int maxHp, int cargo, int maxCargo, int nanoHull, int maxNanoHull, int x,
                                     int y, int mapID, int factionID, int pClanID, int expansionStage, bool premium,
                                     double experience, double honor, short level, long credits, long uridium,
                                     float jackpot, int rankID, String clanTag, int rings, bool unknown2,
                                     bool cloaked, bool unknown3, List<VisualModifierCommand> pModifiers)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(factionID << 12 | factionID >> 20);
            param1.writeUTF(shipType);
            param1.writeBoolean(unknown3);
            param1.writeShort(-22408);
            param1.writeInt(maxCargo >> 16 | maxCargo << 16);
            param1.writeInt(level << 9 | level >> 23);
            param1.writeInt(maxNanoHull >> 14 | maxNanoHull << 18);
            param1.writeBoolean(cloaked);
            param1.writeInt(speed >> 10 | speed << 22);
            param1.writeDouble(uridium);
            param1.writeInt(mapID >> 9 | mapID << 23);
            param1.writeInt(shield >> 10 | shield << 22);
            param1.writeInt(expansionStage << 8 | expansionStage >> 24);
            param1.writeDouble(credits);
            param1.writeInt(nanoHull >> 13 | nanoHull << 19);
            param1.writeUTF(username);
            param1.writeInt(x >> 5 | x << 27);
            param1.writeDouble(experience);
            param1.writeBoolean(premium);
            param1.writeUTF(clanTag);
            param1.writeInt(cargo >> 10 | cargo << 22);
            param1.writeInt(maxShield << 6 | maxShield >> 26);
            param1.writeInt(maxHp >> 5 | maxHp << 27);
            param1.writeInt(userID << 1 | userID >> 31);
            param1.writeInt(rings >> 1 | rings << 31);
            param1.writeDouble(honor);
            param1.writeInt(pClanID >> 8 | pClanID << 24);
            param1.writeInt(rankID << 3 | rankID >> 29);
            param1.writeFloat(jackpot);
            param1.writeInt(hp << 3 | hp >> 29);
            param1.writeInt(pModifiers.Count);
            foreach(var modifier in pModifiers)
            {
                param1.write(modifier.write());
            }
            param1.writeInt(y << 1 | y >> 31);
            param1.writeBoolean(unknown2);
            return param1.ToByteArray();
        }
    }
}
