using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AsteroidProgressCommand
    {
        public const short ID = 1995;

        public String bestProgressClanName = "";     
        public float bestProgress = 0;     
        public Boolean buildButtonActive = false;      
        public String ownClanName = "";     
        public EquippedModulesModule state;     
        public int battleStationId = 0;      
        public float ownProgress = 0;

        public AsteroidProgressCommand(int battleStationId, float ownProgress, float bestProgress, string ownClanName, string bestProgressClanName, EquippedModulesModule state, bool buildButtonActive)
        {
            this.battleStationId = battleStationId;
            this.ownProgress = ownProgress;
            this.bestProgress = bestProgress;
            this.ownClanName = ownClanName;
            this.bestProgressClanName = bestProgressClanName;
            this.state = state;
            this.buildButtonActive = buildButtonActive;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(bestProgressClanName);
            param1.writeFloat(bestProgress);
            param1.writeBoolean(buildButtonActive);
            param1.writeUTF(ownClanName);
            param1.write(state.write());
            param1.writeInt(battleStationId << 9 | battleStationId >> 23);
            param1.writeFloat(ownProgress);
            return param1.Message.ToArray();
        }
    }
}
