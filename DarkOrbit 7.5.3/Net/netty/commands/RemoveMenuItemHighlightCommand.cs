using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class RemoveMenuItemHighlightCommand
    {
        public const short ID = 31344;

        public static byte[] write(class_h2P e1d, String itemId, class_K18 Q1U)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.write(e1d.write());
            param1.writeShort(-10359);
            param1.write(Q1U.write());
            param1.writeUTF(itemId);
            return param1.ToByteArray();
        }
    }
}
