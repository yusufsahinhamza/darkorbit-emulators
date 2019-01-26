using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class UbahsModule
    {
        public const short ID = 5776;

        public List<Ubal4bModule> vare3G;

        public UbahsModule(List<Ubal4bModule> param1)
        {
            this.vare3G = param1;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(this.vare3G.Count);
            foreach (var e3G in this.vare3G)
            {
                param1.write(e3G.write());
            }
            return param1.Message.ToArray();
        }
    }
}
