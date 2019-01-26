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
        public const short ID = 6873;

        public PetGearTypeModule gearTypeToActivate;
        public int optParam;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            param1.readShort();
            gearTypeToActivate = new PetGearTypeModule(param1.readShort());
            optParam = param1.readShort();
        }
    }
}
