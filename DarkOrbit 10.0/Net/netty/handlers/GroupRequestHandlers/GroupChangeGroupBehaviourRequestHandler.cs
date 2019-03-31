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
    class GroupChangeGroupBehaviourRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;
            player.Group?.ChangeBehavior(player);
        }
    }
}
