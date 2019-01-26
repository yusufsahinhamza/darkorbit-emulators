using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class class_84I
    {
        public const short ID = 2015;

        public class_84I()
        {
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);

            return param1.Message.ToArray();
        }
    }
}
