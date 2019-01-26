using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AssetCreateCommand
    {
        public static short ID = 19926;

        public static byte[] write(AssetTypeModule type, String userName, int factionId, String clanTag, int assetId, int designId,
                              int expansionStage, int posX, int posY, int clanId, bool invisible, bool visibleOnWarnRadar,
                              bool detectedByWarnRadar, ClanRelationModule clanRelation,
                              List<VisualModifierCommand> modifier)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.write(type.write());
            param1.writeUTF(userName);
            param1.writeInt(factionId);
            param1.writeUTF(clanTag);
            param1.writeInt(assetId);
            param1.writeInt(designId);
            param1.writeInt(expansionStage);
            param1.writeInt(posX);
            param1.writeInt(posY);
            param1.writeInt(clanId);
            param1.writeBoolean(invisible);
            param1.writeBoolean(visibleOnWarnRadar);
            param1.writeBoolean(detectedByWarnRadar);
            param1.write(clanRelation.write());
            param1.writeInt(modifier.Count);
            foreach(var _loc2_ in modifier)
            {
                param1.write(_loc2_.write());
            }
            return param1.ToByteArray();
        }
    }
}
