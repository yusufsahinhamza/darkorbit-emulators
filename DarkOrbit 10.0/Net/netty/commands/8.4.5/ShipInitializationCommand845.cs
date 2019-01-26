using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ShipInitializationCommand845
    {
        public const short ID = 31790;

        public static byte[] write(int userID, String username, String shipType, int speed, int shield, int maxShield,
                                     int hp, int maxHp, int cargo, int maxCargo, int nanoHull, int maxNanoHull, int x,
                                     int y, int mapID, int factionID, int unknown, int expansionStage, bool premium,
                                     double experience, double honor, short level, long credits, long uridium,
                                     double jackpot, int rankID, String clanTag, int rings, bool unknown2,
                                     bool cloaked, bool unknown3, List<VisualModifierCommand845> pModifiers)
        {



            var param1 = new ByteArray(ID);
            param1.writeInt(unknown >> 1 | unknown << 31);//18416
            param1.writeInt(cargo << 4 | cargo >> 28);//2000
            param1.writeDouble(uridium);//851
            param1.writeInt(speed << 6 | speed >> 26);//359
            param1.writeDouble(honor);//80671
            param1.writeInt(mapID >> 7 | mapID << 25);//1
            param1.writeUTF(shipType);//ship_vengeance_design_avenger
            param1.writeShort(24132);
            param1.writeBoolean(premium);//false
            param1.writeInt(pModifiers.Count);//0
            foreach (var c in pModifiers)
            {
                param1.write(c.write());
            }
            param1.writeInt(expansionStage >> 14 | expansionStage << 18);//3
            param1.writeInt(rings << 7 | rings >> 25);//0
            param1.writeInt(shield >> 1 | shield << 31);//66000
            param1.writeInt(maxCargo >> 9 | maxCargo << 23);//2000
            param1.writeInt(userID >> 11 | userID << 21);//163457209
            param1.writeBoolean(unknown3);//true
            param1.writeBoolean(unknown2);//true
            param1.writeInt(y << 5 | y >> 27);//3342
            param1.writeInt(nanoHull >> 5 | nanoHull << 27);//0
            param1.writeInt(maxNanoHull >> 14 | maxNanoHull << 18);//180000
            param1.writeDouble(experience);//19104148
            param1.writeInt(hp >> 11 | hp << 21);//185000
            param1.writeInt(x << 2 | x >> 30);//1774
            param1.writeInt(level >> 14 | level << 18);//12
            param1.writeInt(maxShield >> 10 | maxShield << 22);//66000
            param1.writeFloat((float)jackpot);//172.42999267578125
            param1.writeInt(factionID >> 14 | factionID << 18);//1
            param1.writeUTF(clanTag);//GK
            param1.writeDouble(credits);//23341138
            param1.writeBoolean(cloaked);//false
            param1.writeUTF(username);//
            param1.writeInt(maxHp >> 13 | maxHp << 19);//185000
            param1.writeInt(rankID >> 13 | rankID << 19);//4
            return param1.ToByteArray();
        }
    }
}
