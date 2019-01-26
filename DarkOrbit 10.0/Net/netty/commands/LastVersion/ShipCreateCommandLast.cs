using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ShipCreateCommandLast
    {
        public const short ID = 5794;

        public static byte[] write(int userId, string shipType, int expansionStage, string clanTag, string userName, int x, int y, int factionId, int clanId, int dailyRank, bool warnBox, ClanRelationModuleLast clanDiplomacy, int galaxyGatesDone, bool useSystemFont, bool npc, bool cloaked, int motherShipId, int positionIndex, string varYq, List<VisualModifierCommandLast> modifier, class_q1OLast varv31)
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(shipType);
            param1.writeBoolean(useSystemFont);
            param1.writeInt(x << 1 | x >> 31);
            param1.write(varv31.write());
            param1.writeShort(-27647);
            param1.writeInt(galaxyGatesDone >> 9 | galaxyGatesDone << 23);
            param1.writeBoolean(npc);
            param1.writeUTF(userName);
            param1.writeInt(expansionStage << 8 | expansionStage >> 24);
            param1.writeBoolean(warnBox);
            param1.writeBoolean(cloaked);
            param1.write(clanDiplomacy.write());
            param1.writeUTF(varYq);
            param1.writeInt(dailyRank << 15 | dailyRank >> 17);
            param1.writeInt(y >> 15 | y << 17);
            param1.writeShort(4701);
            param1.writeInt(factionId << 6 | factionId >> 26);
            param1.writeInt(userId >> 5 | userId << 27);
            param1.writeUTF(clanTag);
            param1.writeInt(motherShipId << 11 | motherShipId >> 21);
            param1.writeInt(clanId << 1 | clanId >> 31);
            param1.writeInt(positionIndex >> 2 | positionIndex << 30);
            param1.writeInt(modifier.Count);
            foreach(var m in modifier)
            {
                param1.write(m.write());
            }
            return param1.ToByteArray();
        }
    }
}
