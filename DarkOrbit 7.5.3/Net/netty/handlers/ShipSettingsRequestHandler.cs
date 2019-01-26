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
    class ShipSettingsRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new ShipSettingsRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;

            var shipSettings = player.Settings.ShipSettings;

            shipSettings.quickbarSlots = read.quickbarSlots;
            shipSettings.quickbarSlotsPremium = read.quickbarSlotsPremium;
            shipSettings.selectedRocketLauncher = read.selectedHellstormRocket;
            shipSettings.selectedLaser = read.selectedLaser;
            shipSettings.selectedRocket = read.selectedRocket;

            QueryManager.SavePlayer.Settings(player);
        }
    }
}
