using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ClientUISlotBarsCommand
    {
        public const short ID = 30106;

        public static byte[] write(String param2, List<ClientUISlotBarModule> slotBars,
                                   List<ClientUISlotBarCategoryModule> categories)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(slotBars.Count);
            foreach(var sb in slotBars)
            {
                param1.write(sb.write());
            }
            param1.writeShort(-16701);
            param1.writeInt(categories.Count);
            foreach(var ct in categories)
            {
                param1.write(ct.write());
            }
            param1.writeShort(3664);
            param1.writeUTF(param2);
            return param1.ToByteArray();
        }
    }
}
