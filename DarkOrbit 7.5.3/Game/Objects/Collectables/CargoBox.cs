using Ow.Game.Movements;
using Ow.Managers;
using Ow.Net.netty;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Collectables
{
    class CargoBox : Collectable
    {
        public CargoBox(int collectableId, Position position, Spacemap spacemap, bool respawnable, bool spaceball, Player toPlayer = null) : base(collectableId, position, spacemap, respawnable, toPlayer) { Spaceball = spaceball; }

        private bool Spaceball { get; set; }

        public override void Reward(Player player)
        {
            int experience = 0;
            int honor = 0;
            int uridium = 0;

            if (Spaceball)
            {
                experience = player.Ship.GetExperienceBoost(Randoms.random.Next(0, 25600));
                honor = player.Ship.GetHonorBoost(Randoms.random.Next(0, 256));
                uridium = Randoms.random.Next(0, 256);
            }
            else
            {
                experience =  player.Ship.GetExperienceBoost(Randoms.random.Next(0, 12800));
                honor =  player.Ship.GetHonorBoost(Randoms.random.Next(0, 128));
                uridium =  Randoms.random.Next(0, 128);
            }

            player.ChangeData(DataType.EXPERIENCE, experience);
            player.ChangeData(DataType.HONOR, honor);
            player.ChangeData(DataType.URIDIUM, uridium);
        }
    }
}
