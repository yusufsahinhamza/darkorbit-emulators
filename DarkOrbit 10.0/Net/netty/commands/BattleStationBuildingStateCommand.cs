using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class BattleStationBuildingStateCommand
    {
        public static short ID = 4873;

        public static byte[] write(int mapAssetId, int battleStationId, string battleStationName, int secondsLeft, int totalSeconds, string ownerClan, FactionModule affiliatedFaction)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeUTF(ownerClan);
            param1.writeShort(-21121);
            param1.writeShort(566);
            param1.writeInt(battleStationId << 8 | battleStationId >> 24);
            param1.write(affiliatedFaction.write());
            param1.writeInt(totalSeconds << 4 | totalSeconds >> 28);
            param1.writeInt(secondsLeft << 12 | secondsLeft >> 20);
            param1.writeInt(mapAssetId << 13 | mapAssetId >> 19);
            param1.writeUTF(battleStationName);
            return param1.ToByteArray();
        }
    }
}
