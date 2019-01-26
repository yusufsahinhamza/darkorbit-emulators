using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BattleStationStatusCommand
    {
        public const short ID = 21495;

        public static byte[] write(int mapAssetId, int battleStationId, string battleStationName, bool deflectorShieldActive, int deflectorShieldSeconds, int deflectorShieldSecondsMax, int attackRating, int defenceRating, int repairRating, int honorBoosterRating, int experienceBoosterRating, int damageBoosterRating, int deflectorShieldRate, int repairPrice, EquippedModulesModule equipment, bool unknown)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(deflectorShieldRate >> 9 | deflectorShieldRate << 23);
            param1.writeInt(deflectorShieldSecondsMax << 13 | deflectorShieldSecondsMax >> 19);
            param1.writeInt(attackRating << 14 | attackRating >> 18);
            param1.writeInt(repairRating >> 6 | repairRating << 26);
            param1.writeInt(defenceRating >> 9 | defenceRating << 23);
            param1.writeInt(battleStationId >> 13 | battleStationId << 19);
            param1.writeShort(23737);
            param1.writeInt(mapAssetId >> 10 | mapAssetId << 22);
            param1.writeInt(damageBoosterRating << 3 | damageBoosterRating >> 29);
            param1.writeInt(repairPrice >> 14 | repairPrice << 18);
            param1.write(equipment.write());
            param1.writeInt(deflectorShieldSeconds << 8 | deflectorShieldSeconds >> 24);
            param1.writeUTF(battleStationName);
            param1.writeInt(honorBoosterRating << 4 | honorBoosterRating >> 28);
            param1.writeBoolean(unknown);
            param1.writeBoolean(deflectorShieldActive);
            param1.writeShort(16248);
            param1.writeInt(experienceBoosterRating << 3 | experienceBoosterRating >> 29);
            return param1.ToByteArray();
        }
    }
}
