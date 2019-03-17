using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class UbaG3FModule
    {
        public const short ID = 17524;

        public int varjP = 0;
        public float points = 0;
        public int varp1w = 0;
        public int rank = 0;

        public UbaG3FModule(int victories, int duels, int rank, int points)
        {
            this.varjP = victories;
            this.varp1w = duels;
            this.rank = rank;
            this.points = points;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(this.varjP << 1 | this.varjP >> 31);
            param1.writeFloat(this.points);
            param1.writeInt(this.varp1w >> 12 | this.varp1w << 20);
            param1.writeShort(-21702);
            param1.writeInt(this.rank >> 4 | this.rank << 28);
            return param1.Message.ToArray();
        }
    }
}
