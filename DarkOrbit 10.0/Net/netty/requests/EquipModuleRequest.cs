using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class EquipModuleRequest
    {
        public const short ID = 2520;

        public int battleStationId = 0;     
        public int itemId = 0;     
        public int slotId = 0;     
        public Boolean replace = false;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            this.itemId = parser.readInt();
            this.itemId = this.itemId << 4 | this.itemId >> 28;
            this.replace = parser.readBoolean();
            this.slotId = parser.readInt();
            this.slotId = this.slotId << 5 | this.slotId >> 27;
            this.battleStationId = parser.readInt();
            this.battleStationId = this.battleStationId << 4 | this.battleStationId >> 28;
        }
    }
}
