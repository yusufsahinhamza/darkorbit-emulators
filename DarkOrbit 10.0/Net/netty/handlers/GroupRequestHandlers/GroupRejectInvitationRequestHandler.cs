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
    class GroupRejectInvitationRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new GroupRejectInvitationRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var inviterSession = GameManager.GetGameSession(read.userId);

            if (inviterSession == null) return;
            if (player.Storage.GroupInvites.ContainsKey(inviterSession.Player.Id))
            {
                player.Storage.GroupInvites.Remove(inviterSession.Player.Id);
                inviterSession.Player.SendCommand(GroupRemoveInvitationCommand.write(inviterSession.Player.Id, player.Id, GroupRemoveInvitationCommand.REJECT));
            }
        }
    }
}
