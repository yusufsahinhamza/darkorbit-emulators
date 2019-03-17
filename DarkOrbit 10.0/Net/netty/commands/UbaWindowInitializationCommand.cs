using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class UbaWindowInitializationCommand
    {
        public const short ID = 21125;

        public const short varc4R = 6;      
        public const short varf2n = 3;     
        public const short varo2q = 4;      
        public const short varSK = 2;      
        public const short varr1v = 1;      
        public const short var6J = 5;      
        public const short var61X = 0;

        public static byte[] write(command_NQ varxV, short varx6)
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(-11683);
            param1.writeShort(varx6);
            param1.write(varxV.write());
            return param1.ToByteArray();
        }
    }
}
