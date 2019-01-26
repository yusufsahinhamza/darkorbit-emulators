using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class AbilityLaunchRequest
    {
        public const short ID = 26418;

        public int selectedAbilityId;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            selectedAbilityId = param1.readInt();
        }
    }
}
