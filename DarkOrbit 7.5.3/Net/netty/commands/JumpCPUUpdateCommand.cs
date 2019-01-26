using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class JumpCPUUpdateCommand
    {
        public const short ID = 19147;

        public static byte[] write(List<JumpCPUPriceMappingModule> priceList)
        {
            var param1 = new ByteArray(ID);
            param1.writeInt(priceList.Count);
            foreach(var price in priceList)
            {
                param1.write(price.write());
            }
            return param1.ToByteArray();
        }
    }
}
