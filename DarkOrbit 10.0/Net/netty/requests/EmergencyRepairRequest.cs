using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class EmergencyRepairRequest
    {
        public const short ID = 26941;

        public int slotId = 0;
        public int battleStationId = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            this.slotId = parser.readInt();
            this.slotId = (int)(((uint)this.slotId) << 4 | ((uint)this.slotId >> 28));
            this.battleStationId = parser.readInt();
            this.battleStationId = (int)(((uint)this.battleStationId) << 5 | ((uint)this.battleStationId >> 27));
        }
    }
}
