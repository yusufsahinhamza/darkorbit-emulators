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
        public const short ID = 29819;

        public static byte[] write(int userId, int x, int y, int timeToTarget)
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(6455);
            param1.writeInt(y >> 13 | y << 19);
            param1.writeInt(userId >> 12 | userId << 20);
            param1.writeInt(x >> 4 | x << 28);
            param1.writeInt(timeToTarget >> 13 | timeToTarget << 19);
            return param1.ToByteArray();
        }
    }
}
