using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_O4f
    {
        public const short ID = 26611;

        public List<class_84I> attributes;     
        public int varw37 = 0;

        public class_O4f(int varw37, List<class_84I> attributes)
        {
            this.varw37 = varw37;
            this.attributes = attributes;
        }

        public byte[] write()
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(this.attributes.Count);
            foreach(var attribute in this.attributes)
            {
                param1.write(attribute.write());
            }
            param1.writeInt(this.varw37 << 4 | this.varw37 >> 28);
            param1.writeShort(-32069);
            return param1.Message.ToArray();
        }
    }
}
