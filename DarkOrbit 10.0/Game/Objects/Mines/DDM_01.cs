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
        public DDM_01(Player player, Spacemap spacemap, Position position, int mineTypeId) : base(player, spacemap, position, mineTypeId, player.Settings.InGameSettings.selectedFormation == DroneManager.LANCE_FORMATION) { }

        public override void Explode()
        {
            foreach (var character in Spacemap.Characters.Values)
            {
                if (character is Player player && player.Position.DistanceTo(Position) < EXPLODE_RANGE)
                {
                    if (Player == player || player.Storage.DuelOpponent == null || (player.Storage.DuelOpponent != null && Player == player.Storage.DuelOpponent))
                    {
                        var damage = Maths.GetPercentage(player.MaxHitPoints, 20);

                        if (Lance)
                            damage *= 2;

                        AttackManager.Damage(Player, player as Player, DamageType.MINE, damage, true, true, false, false);
                    }
                }
            }
        }
    }
}
