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
    class BonusBox : Collectable
    {
        public BonusBox(Position position, Spacemap spacemap, bool respawnable, Player toPlayer = null) : base(AssetTypeModule.BOXTYPE_BONUS_BOX, position, spacemap, respawnable, toPlayer) { }

        public override void Reward(Player player)
        {
            double rand = Randoms.random.NextDouble();

            if (rand <= 0.40)
            {
                var uridium = Randoms.random.Next(25, 150);
                uridium += Maths.GetPercentage(uridium, player.GetSkillPercentage("Luck"));

                player.ChangeData(DataType.URIDIUM, uridium);
            }
            else
            {
                var credits = Randoms.random.Next(150, 15000);
                player.ChangeData(DataType.CREDITS, credits);
            }
        }

        public override byte[] GetCollectableCreateCommand()
        {
            return CreateBoxCommand.write("BONUS_BOX", Hash, Position.Y, Position.X);
        }
    }
}
