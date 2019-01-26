using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class LegacyModuleRequest
    {
        public const short ID = 29052;

        public string message;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            message = param1.readUTF();
        }
    }
}
