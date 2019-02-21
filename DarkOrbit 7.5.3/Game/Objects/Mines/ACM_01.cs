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
        public ACM_01(Player player, Spacemap spacemap, Position position, int mineTypeId) : base(player, spacemap, position, mineTypeId, player.SettingsManager.SelectedFormation == DroneManager.LANCE_FORMATION) { }

        public override void Explode()
        {
            foreach (var characters in Spacemap.Characters.Values)
            {
                if (characters is Player player && player.Position.DistanceTo(Position) < EXPLODE_RANGE)
                {
                    var damage = Maths.GetPercentage(player.CurrentHitPoints, 20);

                    if (Lance)
                        damage *= 2;

                    Player.AttackManager.Damage(Player, player, DamageType.MINE, damage, 0, false);
                }
            }
        }
    }
}
