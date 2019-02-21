using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class LevelUpCommand
    {
        public static short ID = 32247;

        public static byte[] write(int uid, int newLevel)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeInt(uid);
            param1.writeInt(newLevel);
            return param1.ToByteArray();
        }
    }
}