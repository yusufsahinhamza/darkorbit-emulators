using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests.BattleStationRequests
{
    class UnEquipModuleRequest
    {
        public const short ID = 16131;

        public int itemId = 0;    
        public int battleStationId = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            this.battleStationId = parser.readInt();
            this.battleStationId = (int)(((uint)this.battleStationId) << 5 | ((uint)this.battleStationId >> 27));
            parser.readShort();
            parser.readShort();
            this.itemId = parser.readInt();
            this.itemId = (int)(((uint)this.itemId) << 12 | ((uint)this.itemId >> 20));
        }
    }
}
