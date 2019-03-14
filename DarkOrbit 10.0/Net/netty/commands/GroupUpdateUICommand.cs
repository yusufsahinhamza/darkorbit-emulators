using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupUpdateUICommand
    {
        public const short ID = 14575;

        public static byte[] write(int userId, List<command_i3O> updates)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeShort(-24867);
            param1.writeInt(userId >> 15 | userId << 17);
            param1.writeInt(updates.Count);
            foreach(var _loc2_ in updates)
            {
                param1.write(_loc2_.write());
            }
            return param1.ToByteArray();
        }
    }
}
