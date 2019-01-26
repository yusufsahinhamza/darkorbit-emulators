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
        public static short ID = 13718;

        public static byte[] write(AssetTypeModule type, String name, int factionId, String clanTag, int assetId, int designId,
                              int expansionStage, int x, int y, int clanId, bool invisible, bool visibleOnWarnRadar,
                              bool detectedByWarnRadar, bool showBubble, ClanRelationModule clanRelation,
                              List<VisualModifierCommand> modifier)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(x >> 10 | x << 22);
            param1.writeInt(y >> 1 | y << 31);
            param1.writeUTF(name);
            param1.writeInt(modifier.Count);
            foreach(var m in modifier)
            {
                param1.write(m.write());
            }
            param1.writeBoolean(showBubble);
            param1.writeInt(expansionStage << 15 | expansionStage >> 17);
            param1.writeInt(designId << 16 | designId >> 16);
            param1.writeBoolean(invisible);
            param1.writeInt(factionId >> 3 | factionId << 29);
            param1.writeBoolean(visibleOnWarnRadar);
            param1.writeShort(24441);
            param1.write(clanRelation.write());
            param1.writeUTF(clanTag);
            param1.write(type.write());
            param1.writeInt(clanId >> 10 | clanId << 22);
            param1.writeBoolean(detectedByWarnRadar);
            param1.writeInt(assetId >> 5 | assetId << 27);
            return param1.ToByteArray();
        }
    }
}
