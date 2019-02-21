using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PetHarvestEffectCommand
    {
        public const short ID = 7212;

        public static byte[] write()
        {
            var param1 = new ByteArray(ID);
            return param1.ToByteArray();
        }
    }
}
