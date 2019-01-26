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
        public const short ID = 6532;

        public int bilmiyorum1 = 0;      
        public int bilmiyorum2 = 0;      
        public int bilmiyorum3 = 0;      
        public String hash = "";      
        public int bilmiyorum4 = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            bilmiyorum1 = parser.readInt();
            bilmiyorum1 = bilmiyorum1 >> 3 | bilmiyorum1 << 29;
            bilmiyorum4 = parser.readInt();
            bilmiyorum4 = bilmiyorum4 >> 2 | bilmiyorum4 << 30;
            bilmiyorum2 = parser.readInt();
            bilmiyorum2 = bilmiyorum2 << 14 | bilmiyorum2 >> 18;
            bilmiyorum3 = parser.readInt();
            bilmiyorum3 = bilmiyorum3 >> 1 | bilmiyorum3 << 31;
            hash = parser.readUTF();
        }
    }
}
