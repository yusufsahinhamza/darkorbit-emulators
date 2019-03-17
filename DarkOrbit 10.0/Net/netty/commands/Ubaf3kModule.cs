using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.commands
{
    class Ubaf3kModule : command_j3s
    {
        public const short ID = 18631;

        public string lootId = "";
        public int amount = 0;

        public Ubaf3kModule(string lootId, int amount)
        {
            this.lootId = lootId;
            this.amount = amount;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeUTF(this.lootId);
            param1.writeShort(31663);
            param1.writeInt(this.amount >> 2 | this.amount << 30);
            return param1.Message.ToArray();
        }
    }
}
