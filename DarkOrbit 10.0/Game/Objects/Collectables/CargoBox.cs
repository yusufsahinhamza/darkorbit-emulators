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
        public CargoBox(Position position, Spacemap spacemap, bool respawnable, bool spaceball, Player toPlayer = null) : base(AssetTypeModule.BOXTYPE_FROM_SHIP, position, spacemap, respawnable, toPlayer) { Spaceball = spaceball; }

        private bool Spaceball { get; set; }

        public override void Reward(Player player)
        {
            int experience = 0;
            int honor = 0;
            int uridium = 0;
            int credits = 0;

            if (Spaceball)
            {
                experience = player.Ship.GetExperienceBoost(Randoms.random.Next(2500, 25600));
                honor = player.Ship.GetHonorBoost(Randoms.random.Next(25, 256));
                uridium = Randoms.random.Next(25, 256);
                credits = Randoms.random.Next(150, 15000);
            }
            else
            {
                experience = player.Ship.GetExperienceBoost(Randoms.random.Next(2500, 12800));
                honor = player.Ship.GetHonorBoost(Randoms.random.Next(25, 128));
                uridium = Randoms.random.Next(25, 128);
                credits = Randoms.random.Next(150, 7500);
            }

            player.ChangeData(DataType.EXPERIENCE, experience);
            player.ChangeData(DataType.HONOR, honor);
            player.ChangeData(DataType.URIDIUM, uridium);
            player.ChangeData(DataType.CREDITS, credits);
        }

        public override byte[] GetCollectableCreateCommand()
        {
            return CreateBoxCommand.write("FROM_SHIP", Hash, Position.Y, Position.X);
        }
    }
}
