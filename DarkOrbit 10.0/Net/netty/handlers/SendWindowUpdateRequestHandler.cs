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
    class SendWindowUpdateRequestHandler : IHandler
    {
        private static List<string> alwaysNotMaximized = new List<string>()
        {
            "logout",
            "settings"
        };

        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new SendWindowUpdateRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var windowSettings = player.Settings.Window;

            if (windowSettings.windows.ContainsKey(read.itemId))
            {
                windowSettings.windows[read.itemId].height = read.height;
                windowSettings.windows[read.itemId].width = read.width;
                windowSettings.windows[read.itemId].x = read.x;
                windowSettings.windows[read.itemId].y = read.y;

                if (!alwaysNotMaximized.Contains(read.itemId))
                    windowSettings.windows[read.itemId].maximixed = read.maximized;

                QueryManager.SavePlayer.Settings(player, "window", windowSettings);
            }
        }
    }
}
