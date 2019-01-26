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
        public const short ID = 4224;

        public string message;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            message = parser.readUTF();
        }
    }
}
