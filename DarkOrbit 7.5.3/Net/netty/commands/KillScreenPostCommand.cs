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
        public const short ID = 4800;

        public static byte[] write(string killerName, string killerEpppLink, string playerShipAlias, DestructionTypeModule cause, List<KillScreenOptionModule> options)
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(killerName);
            param1.writeUTF(killerEpppLink);
            param1.writeUTF(playerShipAlias);
            param1.write(cause.write());
            param1.writeInt(options.Count);
            foreach(var _loc2_ in options)
            {
                param1.write(_loc2_.write());
            }
            return param1.ToByteArray();
        }
    }
}
