using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players
{
    class Attacker
    {
        public Player Player { get; set; }

        public DateTime LastRefresh { get; private set; }

        public bool FadedToGray { get; set; }

        public Attacker(Player player) { Player = player; LastRefresh = DateTime.Now; }

        public void Refresh()
        {
            LastRefresh = DateTime.Now;
        }
    }
}
