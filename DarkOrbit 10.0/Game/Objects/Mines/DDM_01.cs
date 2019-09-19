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
    class DDM_01 : Mine
    {
        public DDM_01(Player player, Spacemap spacemap, Position position, int mineTypeId) : base(player, spacemap, position, mineTypeId) { }

        public override void Action(Player player)
        {
            var damage = Maths.GetPercentage(player.MaxHitPoints, 20);
            damage += Maths.GetPercentage(damage, SettingsManager.GetSkillPercentage("Detonation 1", Player.SkillTree.Detonation1));
            damage += Maths.GetPercentage(damage, SettingsManager.GetSkillPercentage("Detonation 2", Player.SkillTree.Detonation2));

            if (Lance)
                damage += Maths.GetPercentage(damage, 50);

            AttackManager.Damage(Player, player as Player, DamageType.MINE, damage, true, true, false, false);
        }
    }
}
