using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.requests
{
    class DroneFormationChangeRequest
    {
        public const short ID = 22456;
 
        public int selectedFormationId;

        public void readCommand(byte[] bytes)
        {
            var param1 = new ByteParser(bytes);
            this.selectedFormationId = param1.readInt();
        }
    }
}
