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
        public const short ID = 24858;

        public static byte[] write(int userId, int shipType, int expansionStage, string clanTag, string userName, int x, int y, 
            int factionId, int clanId, int dailyRank, bool warnBox, ClanRelationModule clanDiplomacy, int galaxyGatesDone, bool useSystemFont, bool npc, bool cloaked, int motherShipId, int positionIndex, List<VisualModifierCommand> modifier)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(userId);
            param1.writeInt(shipType);
            param1.writeInt(expansionStage);
            param1.writeUTF(clanTag);
            param1.writeUTF(userName);
            param1.writeInt(x);
            param1.writeInt(y);
            param1.writeInt(factionId);
            param1.writeInt(clanId);
            param1.writeInt(dailyRank);
            param1.writeBoolean(warnBox);
            param1.write(clanDiplomacy.write());
            param1.writeInt(galaxyGatesDone);
            param1.writeBoolean(useSystemFont);
            param1.writeBoolean(npc);
            param1.writeBoolean(cloaked);
            param1.writeInt(motherShipId);
            param1.writeInt(positionIndex);
            param1.writeInt(modifier.Count);
            foreach(var _loc2_ in modifier)
            {
                param1.write(_loc2_.write());
            }
            return param1.ToByteArray();
        }
    }
}
