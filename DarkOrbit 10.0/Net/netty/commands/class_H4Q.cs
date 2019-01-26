using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_H4Q
    {
        public const short ID = 25086;

        public Boolean varz3l = false;

        public class_H4Q(bool varz3l)
        {
            this.varz3l = varz3l;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(-26630);
            param1.writeBoolean(this.varz3l);
            return param1.Message.ToArray();
        }
    }
}
