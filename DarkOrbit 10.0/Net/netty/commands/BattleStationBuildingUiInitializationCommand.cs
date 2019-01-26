using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BattleStationBuildingUiInitializationCommand
    {
        public const short ID = 770;

        public static byte[] write(int mapAssetId, int battleStationId, string battleStationName, AsteroidProgressCommand progress, AvailableModulesCommand availableModules, int buildTimeInMinutesMin, int buildTimeInMinutesMax, int buildTimeInMinutesIncrement)
        {
            var param1 = new ByteArray(ID);
            param1.write(progress.write());
            param1.writeInt(buildTimeInMinutesMax << 4 | buildTimeInMinutesMax >> 28);
            param1.writeUTF(battleStationName);
            param1.writeInt(mapAssetId << 14 | mapAssetId >> 18);
            param1.writeInt(buildTimeInMinutesMin >> 14 | buildTimeInMinutesMin << 18);
            param1.writeInt(buildTimeInMinutesIncrement << 14 | buildTimeInMinutesIncrement >> 18);
            param1.writeInt(battleStationId << 3 | battleStationId >> 29);
            param1.writeShort(-7693);
            param1.write(availableModules.write());
            return param1.ToByteArray();
        }
    }
}
