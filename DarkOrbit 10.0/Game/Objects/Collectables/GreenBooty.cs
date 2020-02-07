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
        public GreenBooty(Position position, Spacemap spacemap, bool respawnable, Player toPlayer = null) : base(AssetTypeModule.BOXTYPE_PIRATE_BOOTY, position, spacemap, respawnable, toPlayer) { }

        public override void Reward(Player player)
        {
           
                double rand = Randoms.random.NextDouble();

                rand = 0.15;

                if (rand <= 0.02)
                {
                    double rand2 = Randoms.random.NextDouble();

                    if (rand2 <= 0.5)
                    {
                        //test = "lf4";
                    }
                    else
                    {
                        //test = "apis";
                    }
                }
                else if (rand <= 0.09)
                {
                    //test = "gemi tasarım varsa 10k uri";
                }
                else if (rand <= 0.15)
                {
                    var hours = Randoms.random.NextDouble() <= 0.1 ? 10 : 1;
                    var boosterTypes = Randoms.random.NextDouble() <= 0.25 ? new int[] { 1, 16, 9, 11, 6, 3 } : new int[] { 0, 15, 8, 10, 5, 2 };
                    var boosterType = boosterTypes[Randoms.random.Next(boosterTypes.Length)];

                    player.BoosterManager.Add((BoosterType)boosterType, hours);
                }
                else if (rand <= 0.4)
                {
                    var logdisk = Randoms.random.Next(1, 10);
                }
                else
                {
                    var uridium = Randoms.random.Next(1000, 4000);
                    player.ChangeData(DataType.URIDIUM, uridium);
                    QueryManager.SavePlayer.Information(player);
                }

                player.Equipment.Items.BootyKeys--;

                player.SendPacket($"0|A|BK|{player.Equipment.Items.BootyKeys}");
           
        }

        public override byte[] GetCollectableCreateCommand()
        {
            return CreateBoxCommand.write("PIRATE_BOOTY", Hash, Position.Y, Position.X);
        }
    }
}
