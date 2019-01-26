using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class SelectBatteryRequest
    {
        public const short ID = 10575;

        public AmmunitionTypeModule batteryType;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            param1.readShort();
            batteryType = new AmmunitionTypeModule(param1.readShort());
        }
    }
}
