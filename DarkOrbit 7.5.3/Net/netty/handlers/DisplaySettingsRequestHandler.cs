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
    class DisplaySettingsRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new DisplaySettingsRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var displaySettings = player.Settings.Display;

            if (displaySettings.displayDrones != read.displayDrones)
                player.DroneManager.UpdateDrones();

            displaySettings.notSet = false;
            displaySettings.allowAutoQuality = read.useAutoQuality;
            displaySettings.displayBonusBoxes = read.displayBoxes;
            displaySettings.displayChat = read.displayChat;
            displaySettings.displayDrones = read.displayDrones;
            displaySettings.displayFreeCargoBoxes = read.displayCargoboxes;
            displaySettings.displayHitpointBubbles = read.displayHitpointBubbles;
            displaySettings.displayNotFreeCargoBoxes = read.displayPenaltyCargoboxes;
            displaySettings.displayNotifications = read.displayNotifications;
            displaySettings.displayPlayerNames = read.displayPlayerName;
            displaySettings.displayResources = read.displayResources;
            displaySettings.displayWindowsBackground = read.displayWindowBackground;
            displaySettings.dragWindowsAlways = read.alwaysDraggableWindows;
            displaySettings.hoverShips = read.shipHovering;
            displaySettings.preloadUserShips = read.preloadUserShips;
            displaySettings.showPremiumQuickslotBar = read.showSecondQuickslotBar;

            QueryManager.SavePlayer.Settings(player);
        }
    }
}
