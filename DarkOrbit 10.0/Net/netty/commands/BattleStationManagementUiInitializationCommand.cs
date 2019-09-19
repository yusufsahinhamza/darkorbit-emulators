using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BattleStationManagementUiInitializationCommand
    {
        public const short ID = 30947;

        public static byte[] write(int mapAssetId, int battleStationId, string battleStationName, string clanName, FactionModule faction, BattleStationStatusCommand state, AvailableModulesCommand availableModules, int deflectorShieldMinutesMin, int deflectorShieldMinutesMax, int deflectorShieldMinutesIncrement, bool deflectorDeactivationPossible)
        {
            var param1 = new ByteArray(ID);
            param1.write(faction.write());
            param1.writeUTF(battleStationName);
            param1.writeInt(mapAssetId >> 8 | mapAssetId << 24);
            param1.writeInt(battleStationId << 5 | battleStationId >> 27);
            param1.writeInt(deflectorShieldMinutesMin >> 9 | deflectorShieldMinutesMin << 23);
            param1.writeInt(deflectorShieldMinutesMax << 10 | deflectorShieldMinutesMax >> 22);
            param1.writeShort(21318);
            param1.writeInt(deflectorShieldMinutesIncrement << 14 | deflectorShieldMinutesIncrement >> 18);
            param1.writeUTF(clanName);
            param1.write(availableModules.write());
            param1.write(state.write());
            param1.writeBoolean(deflectorDeactivationPossible);
            return param1.ToByteArray();
        }
    }
}
