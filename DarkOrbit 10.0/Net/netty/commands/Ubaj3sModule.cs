using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class Ubaj3sModule
    {
        public const short ID = 12173;

        public Ubaj3sModule()
        {
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(18611);
            return param1.Message.ToArray();
        }
    }
}
