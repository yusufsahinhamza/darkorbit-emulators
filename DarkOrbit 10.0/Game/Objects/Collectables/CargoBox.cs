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
            if(Spaceball)
            {
                int experience = player.Ship.GetExperienceBoost(Randoms.random.Next(25600, 102400));
                int honor = player.Ship.GetHonorBoost(Randoms.random.Next(256, 1024));
                int uridium = Randoms.random.Next(256, 1024);

                player.Experience += experience;
                player.Honor += honor;
                player.Uridium += uridium;

                player.SendPacket("0|LM|ST|EP|" + experience + "|" + player.Experience + "|" + player.Level);
                player.SendPacket("0|LM|ST|HON|" + honor + "|" + player.Honor);
                player.SendPacket("0|LM|ST|URI|" + uridium + "|" + player.Uridium);
            }
            else
            {
                int experience = player.Ship.GetExperienceBoost(Randoms.random.Next(0, 25600));
                int honor = player.Ship.GetHonorBoost(Randoms.random.Next(0, 256));
                int uridium = Randoms.random.Next(0, 256);

                player.Experience += experience;
                player.Honor += honor;
                player.Uridium += uridium;

                player.SendPacket("0|LM|ST|EP|" + experience + "|" + player.Experience + "|" + player.Level);
                player.SendPacket("0|LM|ST|HON|" + honor + "|" + player.Honor);
                player.SendPacket("0|LM|ST|URI|" + uridium + "|" + player.Uridium);
            }

            QueryManager.SavePlayer.Information(player);
        }

        public override byte[] GetCollectableCreateCommand()
        {
            return CreateBoxCommand.write("FROM_SHIP", Hash, Position.Y, Position.X);
        }
    }
}
