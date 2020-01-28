using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupPlayerInCombatModule : command_i3O
    {
        public const short ID = 27245;

        public bool inCombat = false;

        public GroupPlayerInCombatModule(bool inCombat)
        {
            this.inCombat = inCombat;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeBoolean(inCombat);
            return param1.Message.ToArray();
        }
    }
}
