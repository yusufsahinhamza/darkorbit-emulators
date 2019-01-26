using Ow.Game;
using Ow.Game.Objects;
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
    class ShipWarpWindowRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;

            var shipList = new List<ShipWarpModule>();
            shipList.Add(new ShipWarpModule(10, 10, "Goliath", 0, 0, 1, "Goliath"));
            shipList.Add(new ShipWarpModule(63, 63, "Solace", 0, 0, 2, "Solace"));
            shipList.Add(new ShipWarpModule(64, 64, "Diminisher", 0, 0, 3, "Diminisher"));
            shipList.Add(new ShipWarpModule(65, 65, "Spectrum", 0, 0, 4, "Spectrum"));
            shipList.Add(new ShipWarpModule(66, 66, "Sentinel", 0, 0, 5, "Sentinel"));
            shipList.Add(new ShipWarpModule(67, 67, "Venom", 0, 0, 6, "Venom"));
            shipList.Add(new ShipWarpModule(56, 56, "Enforcer", 0, 0, 7, "Enforcer"));
            shipList.Add(new ShipWarpModule(59, 59, "Bastion", 0, 0, 8, "Bastion"));
            shipList.Add(new ShipWarpModule(61, 61, "Veteran", 0, 0, 9, "Veteran"));
            shipList.Add(new ShipWarpModule(62, 62, "Exalted", 0, 0, 10, "Exalted"));

            player.SendCommand(ShipWarpWindowCommand.write(0, 0, true, shipList));
        }
    }
}
