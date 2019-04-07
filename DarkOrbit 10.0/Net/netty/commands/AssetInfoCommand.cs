using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AssetInfoCommand
    {
        public const short ID = 8437;

        public static byte[] write(int assetId, AssetTypeModule type, int designId, int expansionStage, int hitpoints, int maxHitpoints, bool shielded, int shieldEnergy, int maxShieldEnergy)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(hitpoints << 5 | hitpoints >> 27);
            param1.writeInt(designId >> 8 | designId << 24);
            param1.writeInt(assetId >> 6 | assetId << 26);
            param1.writeInt(expansionStage >> 15 | expansionStage << 17);
            param1.writeInt(shieldEnergy << 15 | shieldEnergy >> 17);
            param1.writeInt(maxHitpoints << 15 | maxHitpoints >> 17);
            param1.writeShort(14279);
            param1.write(type.write());
            param1.writeBoolean(shielded);
            param1.writeInt(maxShieldEnergy << 12 | maxShieldEnergy >> 20);
            return param1.ToByteArray();
        }
    }
}
