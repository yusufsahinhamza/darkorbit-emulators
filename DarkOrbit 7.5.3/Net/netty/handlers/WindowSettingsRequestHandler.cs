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
    class WindowSettingsRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new WindowSettingsRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var windowSettings = player.Settings.Window;

            windowSettings.notSet = false;
            windowSettings.clientResolutionId = read.clientResolutionId;
            windowSettings.windowSettings = read.windowSettings;
            windowSettings.resizableWindows = read.resizableWindows;
            windowSettings.minmapScale = read.minimapScale;
            windowSettings.mainmenuPosition = read.mainmenuPosition;
            windowSettings.slotmenuPosition = read.slotmenuPosition;
            windowSettings.slotMenuOrder = read.slotMenuOrder;
            windowSettings.slotMenuPremiumPosition = read.slotmenuPremiumPosition;
            windowSettings.slotMenuPremiumOrder = read.slotMenuPremiumOrder;
            windowSettings.barStatus = read.barStatus;

            QueryManager.SavePlayer.Settings(player);
        }
    }
}
