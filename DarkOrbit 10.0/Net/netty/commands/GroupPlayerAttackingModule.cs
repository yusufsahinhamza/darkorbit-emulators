using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net.netty.commands
{
    class GroupPlayerAttackingModule : command_i3O
    {
        public const short ID = 27245;

        public bool attacking = false;

        public GroupPlayerAttackingModule(bool attacking)
        {
            this.attacking = attacking;
        }

        public override byte[] write()
        {
            var param1 = new ByteArray(ID);
            super(param1);
            param1.writeBoolean(attacking);
            return param1.Message.ToArray();
        }
    }
}
