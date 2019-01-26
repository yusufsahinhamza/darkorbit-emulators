using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class PlayGenericSoundCommand
    {
        public static short ID = 20985;

        public const short varR4l = 3;      
        public const short varM4u = 2;      
        public const short varP3y = 1;      
        public const short varB4D = 0;

        public static byte[] write(short varj4z)
        {
            ByteArray param1 = new ByteArray(ID);
            param1.writeShort(-24180);
            param1.writeShort(varj4z);
            param1.writeShort(-31119);
            return param1.ToByteArray();
        }
    }
}
