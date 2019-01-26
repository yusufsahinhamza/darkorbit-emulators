using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class KillScreenPostCommand
    {
        public const short ID = 10114;

        public static byte[] write(string pKillerName, string pKillerEpppLink, string pPlayerShipAlias,
                                 DestructionTypeModule pCause, List<KillScreenOptionModule> pOptions)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(pOptions.Count);
            foreach(var opt in pOptions)
            {
                param1.write(opt.write());
            }
            param1.writeUTF(pKillerEpppLink);
            param1.write(pCause.write());
            param1.writeUTF(pPlayerShipAlias);
            param1.writeUTF(pKillerName);
            param1.writeShort(7114);
            return param1.ToByteArray();
        }
    }
}
