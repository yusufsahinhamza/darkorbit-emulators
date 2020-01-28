using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupPlayerDisconnectedModule : command_i3O
    {
        public const short ID = 30963;

        public bool disconnected = false;

        public GroupPlayerDisconnectedModule(bool disconnected)
        {
            this.disconnected = disconnected;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeBoolean(disconnected);
            param1.writeShort(-32473);
            param1.writeShort(-30202);
            return param1.Message.ToArray();
        }
    }
}
