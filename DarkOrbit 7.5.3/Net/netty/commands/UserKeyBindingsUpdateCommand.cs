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
        public const short ID = 23508;

        public static byte[] write(List<UserKeyBindingsModule> changedKeyBindings, bool remove)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(changedKeyBindings.Count);
            foreach(var ukbm in changedKeyBindings)
            {
                param1.write(ukbm.write());
            }
            param1.writeBoolean(remove);
            return param1.ToByteArray();
        }
    }
}
