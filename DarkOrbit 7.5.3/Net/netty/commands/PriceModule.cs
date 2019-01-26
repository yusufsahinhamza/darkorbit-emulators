using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PriceModule
    {
        public const short ID = 4446;

        public const short URIDIUM = 1;
        public const short CREDITS = 0;

        public int amount = 0;
        public short type = 0;

        public PriceModule(short type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(this.type);
            param1.writeInt(this.amount);
            return param1.Message.ToArray();
        }
    }
}
