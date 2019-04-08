using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class UbaD26Module : command_NQ
    {
        public const short ID = 21233;

        public UbaD26Module() { }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            return param1.Message.ToArray();
        }
    }
}
