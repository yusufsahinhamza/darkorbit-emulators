using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class PetGearActivationRequest
    {
        public const short ID = 31849;
 
        public short optParam1 = 0;
        public short optParam2 = 0;
        public short optParam3 = 0;
        public short gearId = 0;
        public short optParam5 = 0;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            optParam1 = parser.readShort();
            optParam2 = parser.readShort();
            optParam3 = parser.readShort();
            gearId = parser.readShort();
            optParam5 = parser.readShort();
        }
    }
}
