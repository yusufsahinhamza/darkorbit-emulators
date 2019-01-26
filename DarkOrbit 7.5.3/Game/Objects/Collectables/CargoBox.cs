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

                player.Data.experience += experience;
                player.Data.honor += honor;
                player.Data.uridium += uridium;

                player.SendPacket("0|LM|ST|EP|" + experience + "|" + player.Data.experience + "|" + player.Level);
                player.SendPacket("0|LM|ST|HON|" + honor + "|" + player.Data.honor);
                player.SendPacket("0|LM|ST|URI|" + uridium + "|" + player.Data.uridium);
            }
            else
            {
                int experience = player.Ship.GetExperienceBoost(Randoms.random.Next(0, 25600));
                int honor = player.Ship.GetHonorBoost(Randoms.random.Next(0, 256));
                int uridium = Randoms.random.Next(0, 256);

                player.Data.experience += experience;
                player.Data.honor += honor;
                player.Data.uridium += uridium;

                player.SendPacket("0|LM|ST|EP|" + experience + "|" + player.Data.experience + "|" + player.Level);
                player.SendPacket("0|LM|ST|HON|" + honor + "|" + player.Data.honor);
                player.SendPacket("0|LM|ST|URI|" + uridium + "|" + player.Data.uridium);
            }

            QueryManager.SavePlayer.Information(player);
        }
    }
}
