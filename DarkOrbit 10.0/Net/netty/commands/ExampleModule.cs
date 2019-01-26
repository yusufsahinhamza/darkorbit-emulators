using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class ExampleModule
    {
        public const short ID = 0;

        public ExampleModule()
        {

        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);

            return param1.Message.ToArray();
        }
    }
}
