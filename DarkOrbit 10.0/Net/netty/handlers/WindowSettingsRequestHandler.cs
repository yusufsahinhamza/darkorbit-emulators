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

            windowSettings.barState = read.barStatesAsString;
            windowSettings.hideAllWindows = read.hideAllWindows;
            windowSettings.scale = read.scaleFactor;

            QueryManager.SavePlayer.Settings(player, "window", windowSettings);
        }
    }
}
