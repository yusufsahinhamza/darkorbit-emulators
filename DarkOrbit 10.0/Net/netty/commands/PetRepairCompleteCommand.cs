using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetRepairCompleteCommand
    {
        public const short ID = 32334;

        public static byte[] write()
        {
            ByteArray param1 = new ByteArray(ID);
            return param1.ToByteArray();
        }
    }
}
