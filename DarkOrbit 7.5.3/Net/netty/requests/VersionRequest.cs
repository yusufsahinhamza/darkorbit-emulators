using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class VersionRequest
    {
        public const short ID = 666;

        public int major = 0;    
        public String minor = "";     
        public int build = 0;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            this.major = param1.readInt();
            this.minor = param1.readUTF();
            this.build = param1.readInt();
        }
    }
}
