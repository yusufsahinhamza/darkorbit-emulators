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
            
            if (rand <= 0.001)
            {
                //lf4
            }
            else if (rand <= 0.001)
            {

            }
            else if (rand <= 0.001)
            {

            }





            /*
             * 
             * 
             * 
             * [18:16, 15.09.2019] ERSİN: Yeşil kutudan random credi uri
[18:16, 15.09.2019] ERSİN: Booster
[18:16, 15.09.2019] ERSİN: Çok düşük olasılık apis parçası  zeus parçası lf4
             * 
             * 
             * */












            //QueryManager.SavePlayer.Information(player);
        }

        public override byte[] GetCollectableCreateCommand()
        {
            return CreateBoxCommand.write("PIRATE_BOOTY", Hash, Position.Y, Position.X);
        }
    }
}
