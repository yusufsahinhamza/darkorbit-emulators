using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ShipCreateCommand
    {
        public const short ID = 7270;

        public static byte[] write(int userId, string shipType, int expansionStage, string clanTag, string userName, int x, int y, int factionId, int clanId, int dailyRank, bool warnBox, ClanRelationModule clanDiplomacy, int galaxyGatesDone, bool useSystemFont, bool npc, bool cloaked, int motherShipId, int positionIndex, List<VisualModifierCommand> modifier, class_11d var11d)
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(userName);
            param1.writeInt(galaxyGatesDone >> 13 | galaxyGatesDone << 19);
            param1.writeBoolean(useSystemFont);
            param1.writeInt(factionId >> 7 | factionId << 25);
            param1.writeInt(expansionStage << 6 | expansionStage >> 26);
            param1.writeUTF(shipType);
            param1.writeInt(dailyRank >> 4 | dailyRank << 28);
            param1.writeBoolean(npc);
            param1.writeInt(clanId << 3 | clanId >> 29);
            param1.write(clanDiplomacy.write());
            param1.writeBoolean(warnBox);
            param1.writeInt(motherShipId >> 16 | motherShipId << 16);
            param1.writeUTF(clanTag);
            param1.writeInt(modifier.Count);
            foreach(var m in modifier)
            {
                param1.write(m.write());
            }
            param1.writeBoolean(cloaked);
            param1.writeInt(userId >> 14 | userId << 18);
            param1.writeInt(positionIndex << 3 | positionIndex >> 29);
            param1.writeInt(x << 4 | x >> 28);
            param1.write(var11d.write());
            param1.writeInt(y << 9 | y >> 23);
            return param1.ToByteArray();
        }
    }
}
