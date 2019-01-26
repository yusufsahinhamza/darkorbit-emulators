using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class MoveCommand
    {
        public const short ID = 20502;

        public static byte[] write(int userId, int x, int y, int timeToTarget)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(userId);
            param1.writeInt(x);
            param1.writeInt(y);
            param1.writeInt(timeToTarget);
            return param1.ToByteArray();
        }
    }
}
