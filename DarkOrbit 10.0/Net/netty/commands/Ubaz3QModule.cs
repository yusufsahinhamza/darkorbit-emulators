using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class Ubaz3QModule : command_NQ
    {
        public const short ID = 27641;

        public const short varh3Z = 0;
        public const short varC2f = 2;
        public const short varE1L = 1;

        public short type { get; set; }

        public Ubaz3QModule(short type)
        {
            this.type = type;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeShort(type);
            return param1.Message.ToArray();
        }
    }
}
