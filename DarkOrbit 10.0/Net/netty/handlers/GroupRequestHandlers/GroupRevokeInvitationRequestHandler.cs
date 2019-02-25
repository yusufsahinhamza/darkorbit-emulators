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
    class GroupRevokeInvitationRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new GroupRevokeInvitationRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var invitedSession = GameManager.GetGameSession(read.userId);

            if (invitedSession == null) return;
            if (invitedSession.Player.Storage.GroupInvites.ContainsKey(player.Id))
            {
                invitedSession.Player.Storage.GroupInvites.Remove(player.Id);
                invitedSession.Player.SendCommand(GroupRemoveInvitationCommand.write(player.Id, invitedSession.Player.Id, GroupRemoveInvitationCommand.REVOKE));
            }
        }
    }
}
