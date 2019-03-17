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
    class ProActionBarRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new ProActionBarRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var displaySettings = player.Settings.Display;

            displaySettings.proActionBarOpened = read.opened;

            QueryManager.SavePlayer.Settings(player, "display", player.Settings.Display);
        }
    }
}
