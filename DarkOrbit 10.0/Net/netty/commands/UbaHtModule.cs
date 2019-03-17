using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class UbaHtModule
    {
        public const short ID = 28373;

        public string range = "";
        public List<command_j3s> items;

        public UbaHtModule(string param1, List<command_j3s> param2)
        {
            this.range = param1;
            this.items = param2;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeUTF(this.range);
            param1.writeInt(this.items.Count);
            foreach (var item in this.items)
            {
                param1.write(item.write());
            }
            param1.writeShort(20767);
            return param1.Message.ToArray();
        }
    }
}
