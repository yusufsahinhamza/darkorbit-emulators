using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Managers
{
    abstract class AbstractManager
    {
        public Player Player { get; }

        public AbstractManager(Player player)
        {
            Player = player;
        }
    }
}
