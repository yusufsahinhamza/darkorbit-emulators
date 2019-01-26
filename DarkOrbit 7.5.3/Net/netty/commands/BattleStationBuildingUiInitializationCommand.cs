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
        public const short ID = 14733;

        public static byte[] write(int mapAssetId, int battleStationId, string battleStationName, AsteroidProgressCommand progress, AvailableModulesCommand availableModules, int buildTimeInMinutesMin, int buildTimeInMinutesMax, int buildTimeInMinutesIncrement)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(mapAssetId);
            param1.writeInt(battleStationId);
            param1.writeUTF(battleStationName);
            param1.write(progress.write());
            param1.write(availableModules.write());
            param1.writeInt(buildTimeInMinutesMin);
            param1.writeInt(buildTimeInMinutesMax);
            param1.writeInt(buildTimeInMinutesIncrement);
            return param1.ToByteArray();
        }
    }
}
