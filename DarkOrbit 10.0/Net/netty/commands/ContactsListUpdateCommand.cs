using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ContactsListUpdateCommand
    {
        public static short ID = 8689;

        public static byte[] write(class_o3q varQQ, class_g1a varS1p, class_H4Q varE1m)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.write(varE1m.write());
            param1.write(varQQ.write());
            param1.write(varS1p.write());
            return param1.ToByteArray();
        }
    }
}
