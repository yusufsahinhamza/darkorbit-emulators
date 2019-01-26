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
        public static short ID = 19125;

        public static byte[] write(int uid, int selectedFormationId)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeShort(22039);
            param1.writeInt(uid >> 12 | uid << 20);
            param1.writeInt(selectedFormationId << 5 | selectedFormationId >> 27);
            param1.writeShort(3929);
            return param1.ToByteArray();
        }
    }
}