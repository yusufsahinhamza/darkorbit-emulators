using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupPlayerLocationModule : command_i3O
    {
        public const short ID = 20116;

        public int mapId = 0;
        public int y = 0;
        public int x = 0;

        public GroupPlayerLocationModule(int mapId, int x, int y)
        {
            this.mapId = mapId;
            this.x = x;
            this.y = y;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeShort(-28720);
            param1.writeInt(this.mapId << 11 | this.mapId >> 21);
            param1.writeInt(this.y >> 3 | this.y << 29);
            param1.writeInt(this.x >> 10 | this.x << 22);
            return param1.Message.ToArray();
        }
    }
}
