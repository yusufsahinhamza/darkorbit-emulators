using Ow.Game.Movements;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Collectables
{
    class GreenBooty : Collectable
    {
        public GreenBooty(int collectableId, Position position, Spacemap spacemap, bool respawnable, Player toPlayer = null) : base(collectableId, position, spacemap, respawnable, toPlayer) { }

        public override void Reward(Player player)
        {
            var luck = Randoms.random.Next(0, 3);

            if (luck == 1)
            {
                player.BoosterManager.Add(BoosterType.HP_B01, 1);
            }
            else if (luck == 2)
            {
                player.BoosterManager.Add(BoosterType.DMG_B01, 1);
            }
            else
            {
                player.BoosterManager.Add(BoosterType.SHD_B01, 10);
            }
            QueryManager.SavePlayer.Information(player);
        }
    }
}
