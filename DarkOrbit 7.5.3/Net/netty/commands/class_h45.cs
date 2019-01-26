using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_h45
    {
        public const short ID = 11118;

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            return param1.Message.ToArray();
        }
    }
}
