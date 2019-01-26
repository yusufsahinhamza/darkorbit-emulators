using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class DestructionTypeModule
    {
        public const short ID = 19134;

        public const short PLAYER = 0;
        public const short NPC = 1;
        public const short RADITATION = 2;
        public const short MINE = 3;
        public const short MISC = 4;
        public const short BATTLESTATION = 5;

        public short cause = 0;

        public DestructionTypeModule(short pCause)
        {
            this.cause = pCause;
        }

        public byte[] write()
        {
            var param1 = new ByteArray(ID);
            param1.writeShort(26620);
            param1.writeShort(this.cause);
            return param1.Message.ToArray();
        }
    }
}
