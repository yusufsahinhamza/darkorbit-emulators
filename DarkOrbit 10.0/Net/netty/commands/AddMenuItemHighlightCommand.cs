using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AddMenuItemHighlightCommand
    {
        public const short ID = 14222;

        public static byte[] write(class_h2P e1d, String itemId, class_K18 Q1U, class_I1W b3U)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.write(b3U.write());
            param1.write(Q1U.write());
            param1.writeUTF(itemId);
            param1.write(e1d.write());
            return param1.ToByteArray();
        }
    }
}
