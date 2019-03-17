using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class Ubas3wModule : command_NQ
    {
        public const short ID = 14585;

        public UbaG3FModule rank { get; set; }
        public Uba64iModule varA3j { get; set; }
        public UbahsModule vare3G { get; set; }

        public Ubas3wModule(UbaG3FModule rank, Uba64iModule varA3j, UbahsModule vare3G)
        {
            this.rank = rank;
            this.varA3j = varA3j;
            this.vare3G = vare3G;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.write(rank.write());
            param1.writeShort(-9335);
            param1.write(varA3j.write());
            param1.write(vare3G.write());
            return param1.Message.ToArray();
        }
    }
}
