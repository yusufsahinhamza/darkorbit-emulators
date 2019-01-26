using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class UserKeyBindingsUpdateCommand
    {
        public const short ID = 13016;

        public static byte[] write(List<UserKeyBindingsModule> changedKeyBindings, bool remove)
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(remove);
            param1.writeInt(changedKeyBindings.Count);
            foreach(var ukbm in changedKeyBindings)
            {
                param1.write(ukbm.write());
            }
            return param1.ToByteArray();
        }
    }
}
