using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class Uba64iModule
    {
        public const short ID = 13691;

        public double vara4f = 0;
        public List<UbaHtModule> varCp;
        public string vard1r = "";

        public Uba64iModule(string vard1r, double vara4f, List<UbaHtModule> varCp)
        {
            this.vard1r = vard1r;
            this.vara4f = vara4f;
            this.varCp = varCp;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeDouble(this.vara4f);
            param1.writeInt(this.varCp.Count);
            foreach (var cp in this.varCp)
            {
                param1.write(cp.write());
            }
            param1.writeUTF(this.vard1r);
            param1.writeShort(-25482);
            return param1.Message.ToArray();
        }
    }
}
