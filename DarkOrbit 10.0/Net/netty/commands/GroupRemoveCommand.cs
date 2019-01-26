using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupRemoveCommand
    {
        public const short ID = 25158;

        public static byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(31212);
            return param1.ToByteArray();
        }
    }
}
