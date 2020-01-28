using Ow.Game.Events;
using Ow.Game.Movements;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Mines
{
    class ACM_01 : Mine
    {
        public ACM_01(Player player, Spacemap spacemap, Position position, int mineTypeId) : base(player, spacemap, position, mineTypeId) { }

        public override void Action(Player player)
        {
            var damage = Maths.GetPercentage(player.CurrentHitPoints, 20);
            damage += Maths.GetPercentage(damage, player.GetSkillPercentage("Detonation"));

            if (Lance)
                damage += Maths.GetPercentage(damage, 50);

            Player.AttackManager.Damage(Player, player, DamageType.MINE, damage, 0, false);
        }
    }
}
