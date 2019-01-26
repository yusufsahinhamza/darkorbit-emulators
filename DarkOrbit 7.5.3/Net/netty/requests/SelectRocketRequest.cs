using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class SelectRocketRequest
    {
        public const short ID = 15849;

        public AmmunitionTypeModule rocketType;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            param1.readShort();
            rocketType = new AmmunitionTypeModule(param1.readShort());
        }
    }
}
