using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class DroneFormationAvailableFormationsCommand
    {
        public const short ID = 4479;

        public static byte[] write(List<int> availableFormations)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(availableFormations.Count);
            foreach(var _loc2_ in availableFormations)
            {
                param1.writeInt(_loc2_);
            }
            return param1.ToByteArray();
        }
    }
}
