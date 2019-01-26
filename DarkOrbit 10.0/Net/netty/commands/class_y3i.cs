using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_y3i
    {
        public const short ID = 10720;

        public List<class_533> var649;     
        public int varw37 = 0;

        public class_y3i(int varw37, List<class_533> var649)
        {
            this.varw37 = varw37;
            this.var649 = var649;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(this.var649.Count);
            foreach(var _loc2_ in this.var649)
            {
                param1.write(_loc2_.write());
            }
            param1.writeInt(this.varw37 << 2 | this.varw37 >> 30);
            return param1.Message.ToArray();
        }
    }
}
