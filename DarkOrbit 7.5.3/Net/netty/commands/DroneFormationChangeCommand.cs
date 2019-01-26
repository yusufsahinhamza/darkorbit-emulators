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
        public static short ID = 17012;

        public static byte[] write(int uid, int selectedFormationId)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(uid);
            param1.writeInt(selectedFormationId);
            return param1.ToByteArray();
        }
    }
}