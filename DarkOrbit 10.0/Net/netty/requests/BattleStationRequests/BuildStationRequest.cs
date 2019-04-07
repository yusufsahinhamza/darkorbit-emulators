using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests.BattleStationRequests
{
    class BuildStationRequest
    {
        public static short ID = 7044;

        public int battleStationId = 0;
        public int buildTimeInMinutes = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            this.battleStationId = parser.readInt();
            this.battleStationId = (int)(((uint)this.battleStationId) >> 2 | ((uint)this.battleStationId << 30));
            this.buildTimeInMinutes = parser.readInt();
            this.buildTimeInMinutes = (int)(((uint)this.buildTimeInMinutes << 2) | ((uint)this.buildTimeInMinutes >> 30));
        }
    }
}
