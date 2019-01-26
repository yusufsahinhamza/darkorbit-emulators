using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_p2k
    {
        public const short ID = 16435;

        public static byte[] write(List<class_F2I> windows)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeShort(-22711);
            param1.writeShort(-23223);
            param1.writeInt(windows.Count);
            foreach(var w in windows)
            {
                param1.write(w.write());
            }
            return param1.ToByteArray();
        }
    }
}
