using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class KillScreenUpdateCommand
    {
        public const short ID = 9383;

        public static byte[] write(List<KillScreenOptionModule> options)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(options.Count);
            foreach(var _loc2_ in options)
            {
                param1.write(_loc2_.write());
            }
            return param1.ToByteArray();
        }
    }
}
