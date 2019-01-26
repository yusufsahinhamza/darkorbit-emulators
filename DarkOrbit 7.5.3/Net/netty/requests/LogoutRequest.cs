using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class LogoutRequest
    {
        public static short ID = 7987;

        public const short REQUEST_LOGOUT = 0;    
        public const short ABORT_LOGOUT = 1;

        public short request = 0;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            this.request = param1.readShort();
        }
    }
}
