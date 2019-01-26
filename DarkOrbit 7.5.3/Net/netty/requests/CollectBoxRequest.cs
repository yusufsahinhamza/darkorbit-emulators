using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class CollectBoxRequest
    {
        public const short ID = 29347;
   
        public String itemHash = "";      

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            this.itemHash = param1.readUTF();
        }
    }
}
