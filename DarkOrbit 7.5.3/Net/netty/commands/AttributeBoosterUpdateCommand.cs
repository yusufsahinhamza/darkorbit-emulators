using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AttributeBoosterUpdateCommand
    {
        public const short ID = 6518;

        public static byte[] write(List<BoosterUpdateModule> boostedAttributes)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(boostedAttributes.Count);
            foreach(var ba in boostedAttributes)
            {
                param1.write(ba.write());
            }
            return param1.ToByteArray();
        }
    }
}
