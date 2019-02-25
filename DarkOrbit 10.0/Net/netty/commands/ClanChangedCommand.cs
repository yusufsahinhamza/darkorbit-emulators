using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClanChangedCommand
    {
        public const short ID = 26197;

        public static byte[] write(string clanTag, int clanId, int userId)
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(clanTag);
            param1.writeInt(userId << 14 | userId >> 18);
            param1.writeInt(clanId >> 14 | clanId << 18);
            return param1.ToByteArray();
        }
    }
}
