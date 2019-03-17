using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class UbaJ39Module : command_j3s
    {
        public const short ID = 3383;

        public string titleKey = "";

        public UbaJ39Module(string titleKey)
        {
            this.titleKey = titleKey;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeUTF(this.titleKey);
            return param1.Message.ToArray();
        }
    }
}
