using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_I1W
    {
        public const short ID = 3372;

        public int varo1V = 0;
        public bool varl41 = false;

        public class_I1W(bool param1, int param2)
        {
            this.varl41 = param1;
            this.varo1V = param2;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.varl41);
            param1.writeShort(20749);
            param1.writeShort(3016);
            param1.writeInt(this.varo1V << 1 | this.varo1V >> 31);
            return param1.Message.ToArray();
        }
    }
}
