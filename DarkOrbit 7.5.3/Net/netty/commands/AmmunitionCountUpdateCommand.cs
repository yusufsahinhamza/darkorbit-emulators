using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AmmunitionCountUpdateCommand
    {
        public const short ID = 7158;

        public static byte[] write(List<AmmunitionCountModule> ammunitionItems)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(ammunitionItems.Count);
            foreach(var _loc2_ in ammunitionItems)
            {
                param1.write(_loc2_.write());
            }
            return param1.ToByteArray();
        }
    }
}
