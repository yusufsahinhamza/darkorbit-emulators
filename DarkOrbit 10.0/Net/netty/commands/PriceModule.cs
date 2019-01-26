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
        public const short ID = 10487;

        public const short URIDIUM = 1;
        public const short CREDITS = 0;

        public int mAmount = 0;
        public short mType = 0;

        public PriceModule(short pType, int pAmount)
        {
            this.mType = pType;
            this.mAmount = pAmount;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(mType);
            param1.writeInt(mAmount >> 14 | mAmount << 18);
            return param1.Message.ToArray();
        }
    }
}
