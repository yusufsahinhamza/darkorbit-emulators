using Ow.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Clans
{
    class ClanMember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Player Player { get; set; }

        public ClanMember(Player player)
        {
            Player = player;
            Id = Player.Id;
            Name = Player.Name;
        }
    }
}
