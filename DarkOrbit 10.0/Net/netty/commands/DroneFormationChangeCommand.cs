using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class DroneFormationChangeCommand
    {
        public static short ID = 4159;

        public static byte[] write(int uid, int selectedFormationId)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeShort(-29097);
            param1.writeShort(28868);
            param1.writeInt(uid << 2 | uid >> 30);
            param1.writeInt(selectedFormationId >> 10 | selectedFormationId << 22);
            return param1.ToByteArray();
        }
    }
}