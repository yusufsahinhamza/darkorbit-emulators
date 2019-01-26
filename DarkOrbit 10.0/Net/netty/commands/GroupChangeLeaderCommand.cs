using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupChangeLeaderCommand
    {
        public const short ID = 18885;

        public static byte[] write(int userId)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(userId << 2 | userId >> 30);
            return param1.ToByteArray();
        }
    }
}
