using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class AttributeShipSpeedUpdateCommand
    {
        public const short ID = 3657;

        public static byte[] write(int newSpeed)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(newSpeed);
            return param1.ToByteArray();
        }
    }
}
