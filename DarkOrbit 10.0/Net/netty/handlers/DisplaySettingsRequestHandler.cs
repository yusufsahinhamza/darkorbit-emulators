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

            displaySettings.notSet = false;
            displaySettings.allowAutoQuality = read.allowAutoQuality;
            displaySettings.displayBonusBoxes = read.displayBonusBoxes;
            displaySettings.displayChat = read.displayChat;
            displaySettings.displayDrones = read.displayDrones;
            displaySettings.displayFreeCargoBoxes = read.displayFreeCargoBoxes;
            displaySettings.displayHitpointBubbles = read.displayHitpointBubbles;
            displaySettings.displayNotFreeCargoBoxes = read.displayNotFreeCargoBoxes;
            displaySettings.displayNotifications = read.displayNotifications;
            displaySettings.displayPlayerNames = read.displayPlayerNames;
            displaySettings.displayResources = read.displayResources;
            displaySettings.displayWindowsBackground = read.displayWindowsBackground;
            displaySettings.dragWindowsAlways = read.dragWindowsAlways;
            displaySettings.hoverShips = read.hoverShips;
            displaySettings.preloadUserShips = read.preloadUserShips;
            displaySettings.showNotOwnedItems = read.showNotOwnedItems;
            displaySettings.showPremiumQuickslotBar = read.showPremiumQuickslotBar;
            displaySettings.var12P = read.var12P;
            displaySettings.varb3N = read.varb3N;
            displaySettings.displaySetting3DqualityAntialias = read.displaySetting3DqualityAntialias;
            displaySettings.varp3M = read.varp3M;
            displaySettings.displaySetting3DqualityEffects = read.displaySetting3DqualityEffects;
            displaySettings.displaySetting3DqualityLights = read.displaySetting3DqualityLights;
            displaySettings.displaySetting3DqualityTextures = read.displaySetting3DqualityTextures;
            displaySettings.var03r = read.var03r;
            displaySettings.displaySetting3DsizeTextures = read.displaySetting3DsizeTextures;
            displaySettings.displaySetting3DtextureFiltering = read.displaySetting3DtextureFiltering;
            displaySettings.proActionBarKeyboardInputEnabled = read.proActionBarKeyboardInputEnabled;
            displaySettings.proActionBarAutohideEnabled = read.proActionBarAutohideEnabled;

            if(read.proActionBarEnabled != displaySettings.proActionBarEnabled)
            {
                displaySettings.proActionBarEnabled = read.proActionBarEnabled;
                player.SettingsManager.SendSlotBarCommand();
            }

            QueryManager.SavePlayer.Settings(player, "display", displaySettings);
        }
    }
}
