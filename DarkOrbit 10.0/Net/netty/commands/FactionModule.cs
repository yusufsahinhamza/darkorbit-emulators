using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class FactionModule : command_i3O
    {
        public const short ID = 2289;

        public const short MMO = 1;
        public const short VRU = 3;
        public const short EIC = 2;
        public const short NONE = 0;

        public short factionId = 0;

        public FactionModule(short factionId)
        {
            this.factionId = factionId;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeShort(this.factionId);
            param1.writeShort(-31859);
            return param1.Message.ToArray();
        }
    }
}
