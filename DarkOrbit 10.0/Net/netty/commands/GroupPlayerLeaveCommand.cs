using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupPlayerLeaveCommand
    {
        public const short ID = 14175;

        public const short KICK = 2;      
        public const short NONE = 0;    
        public const short LEAVE = 1;

        public static byte[] write(int userId, short reason)
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(18980);
            param1.writeShort(16336);
            param1.writeInt(userId >> 7 | userId << 25);
            param1.writeShort(reason);
            return param1.ToByteArray();
        }
    }
}
