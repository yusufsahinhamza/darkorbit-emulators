using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class ParseFeaturesMenuData
    {
        public const short ID = 11623;

        public static byte[] write(List<ClientUIMenuBarModule> pClientUIMenuBarModuleVector)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(pClientUIMenuBarModuleVector.Count);
            foreach (var c in pClientUIMenuBarModuleVector)
            {
                param1.write(c.write());
            }
            return param1.ToByteArray();
        }
    }
}
