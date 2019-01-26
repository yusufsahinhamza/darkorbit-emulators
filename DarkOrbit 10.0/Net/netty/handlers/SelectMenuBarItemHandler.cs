using Ow.Game;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class SelectMenuBarItemHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new SelectMenuBarItemRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            player.SettingsManager.UseSlotBarItem(read.itemId);
        }
    }
}
