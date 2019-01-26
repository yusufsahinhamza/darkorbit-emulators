using Ow.Game;
using Ow.Game.Objects;
using Ow.Game.Movements;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests.GroupRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers.GroupRequestHandlers
{
    class GroupPingPlayerRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new GroupPingPlayerRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var lockedPlayer = GameManager.GetPlayerById(read.userId);

            if (lockedPlayer == null) return;
            player.Group?.Ping(lockedPlayer.Position);
        }
    }
}
