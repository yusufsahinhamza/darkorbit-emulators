using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class EquipReadyCommand
    {
        public const short ID = 6949;

        public static byte[] write(bool ready)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeBoolean(ready);
            return param1.ToByteArray();
        }
    }
}
