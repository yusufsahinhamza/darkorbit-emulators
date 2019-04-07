using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests.BattleStationRequests
{
    class EquipModuleRequest
    {
        public const short ID = 2520;

        public int itemId = 0;    
        public Boolean replace = false;     
        public int slotId = 0;     
        public int battleStationId = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            this.itemId = parser.readInt();
            this.itemId = (int)(((uint)this.itemId << 4) | ((uint)this.itemId >> 28));
            this.replace = parser.readBoolean();
            this.slotId = parser.readInt();
            this.slotId = (int)(((uint)this.slotId << 5) | ((uint)this.slotId >> 27));
            this.battleStationId = parser.readInt();
            this.battleStationId = (int)(((uint)this.battleStationId << 4) | ((uint)this.battleStationId >> 28));
        }
    }
}
