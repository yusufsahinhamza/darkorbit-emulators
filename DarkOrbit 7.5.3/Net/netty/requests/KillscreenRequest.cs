using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class KillscreenRequest
    {
        public static short ID = 3303;
        public KillScreenOptionTypeModule selection;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            param1.readShort();
            selection = new KillScreenOptionTypeModule(param1.readShort());
        }
    }
}
