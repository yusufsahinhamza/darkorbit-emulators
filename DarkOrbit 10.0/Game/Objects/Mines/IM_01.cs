using Ow.Game.Movements;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Mines
{
    class IM_01 : Mine
    {
        public IM_01(Player player, Spacemap spacemap, Position position, int mineTypeId) : base(player, spacemap, position, mineTypeId) { }

        public override void Action(Player player) { }
    }
}
