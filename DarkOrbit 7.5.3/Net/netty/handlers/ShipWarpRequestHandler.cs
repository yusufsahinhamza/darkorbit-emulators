using Ow.Game;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class ShipWarpRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new ShipWarpRequest();
            read.readCommand(bytes);

            ChangeShip(gameSession, read.shipId);
        }

        public async void ChangeShip(GameSession gameSession, int shipId)
        {
            var player = gameSession.Player;
            player.SendPacket("0|A|STM|msg_swstarted");

            if (!player.AttackingOrUnderAttack() && !player.Moving)
            {
                for (var i = 10; i > 0; i--)
                {
                    var packet = $"0|A|STM|msg_swcountdown_p|%SECONDS%|{i}";
                    player.SendPacket(packet);

                    if (i == 3)
                    {
                        player.AddVisualModifier(new VisualModifierCommand(player.Id, VisualModifierCommand.SHIP_WARP, 0, true));
                    }
                    else if (i <= 1)
                    {
                        player.RemoveVisualModifier(VisualModifierCommand.SHIP_WARP);

                        player.ChangeShip(shipId);

                        player.SendCommand(ShipWarpCompletedCommand.write());
                        player.SendPacket("0|A|STM|msg_switchship_success");
                        //player.AddVisualModifier(new VisualModifierCommand(player.Id, VisualModifierCommand.diver, 0, true));
                    }

                    await Task.Delay(1000);
                }
            }
            else
            {
                player.SendPacket("0|A|STD|Ship warp cancelled");
                player.SendCommand(ShipWarpCanceledCommand.write());
            }

        }
    }
}
