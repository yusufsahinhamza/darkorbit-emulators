using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class UbaM1tModule
    {
        public const short ID = 17423;

        public bool varQ4n = false;

        public UbaM1tModule(bool param1)
        {
            this.varQ4n = param1;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeBoolean(this.varQ4n);
            return param1.Message.ToArray();
        }
    }
}
