using Ow.Game.Movements;
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
    class EMPM_01 : Mine
    {
        public EMPM_01(Player player, Spacemap spacemap, Position position, int mineTypeId) : base(player, spacemap, position, mineTypeId) { }

        public override void Explode()
        {
            foreach (var players in Spacemap.Characters.Values)
            {
                if (players is Player && players.Position.DistanceTo(Position) < RANGE)
                {
                    var player = players as Player;

                    if (player.Invisible && player.Attackable())
                    {
                        player.SendPacket("0|A|STM|msg_uncloaked");
                        player.CpuManager.DisableCloak();
                    }
                }
            }
        }
    }
}
