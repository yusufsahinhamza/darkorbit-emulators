using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class InfiltrationStatusCommand
    {
        public const short ID = 894;

        public static byte[] write(int currentNpcs, int maxNpcs, int secondsRemaining)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(currentNpcs);
            param1.writeInt(maxNpcs);
            param1.writeInt(secondsRemaining);
            return param1.ToByteArray();
        }
    }
}
