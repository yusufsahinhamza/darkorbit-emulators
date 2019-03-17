using Ow.Game;
using Ow.Game.Objects;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class PortalJumpRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;

            var spacemap = player.Spacemap;
            var activatableStationary = player.Spacemap.GetActivatableMapEntity(player.CurrentInRangePortalId);
            var portalMapEntity = (Portal) player.Spacemap.GetActivatableMapEntity(player.CurrentInRangePortalId);

            if (activatableStationary != null && portalMapEntity != null)
            {
                if (spacemap.Options.PvpMap)
                {
                    if(player.LastCombatTime.AddSeconds(10) < DateTime.Now)
                    {
                        portalMapEntity.Click(gameSession);
                    }
                    else
                    {
                        string jumpError = "0|A|STM|jumpgate_failed_pvp_map";
                        player.SendPacket(jumpError);
                    }
                }
                else
                {
                    portalMapEntity.Click(gameSession);
                }
            }
            else
            {
                String warning = "0|A|STM|jumpgate_failed_no_gate";
                player.SendPacket(warning);
            }
        }
    }
}
