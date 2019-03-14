using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupPlayerClanModule : command_i3O
    {
        public const short ID = 2150;

        public int clanId = 0;
        public String clanTag = "";

        public GroupPlayerClanModule(int clanId, string clanTag)
        {
            this.clanId = clanId;
            this.clanTag = clanTag;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeShort(9897);
            param1.writeShort(24769);
            param1.writeInt(this.clanId << 6 | this.clanId >> 26);
            param1.writeUTF(this.clanTag);
            return param1.Message.ToArray();
        }
    }
}
