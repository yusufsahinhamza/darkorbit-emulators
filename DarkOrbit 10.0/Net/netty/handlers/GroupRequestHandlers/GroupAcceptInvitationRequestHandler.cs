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
using Ow.Game.Objects.Players;

namespace Ow.Net.netty.handlers.GroupRequestHandlers
{
    class GroupAcceptInvitationRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new GroupAcceptInvitationRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var inviterPlayer = GameManager.GetPlayerById(read.userId);

            AssembleAcceptedInvitation(player, inviterPlayer);
        }

        public void AssembleAcceptedInvitation(Player player, Player inviterPlayer)
        {
            if (inviterPlayer == null || !player.Storage.GroupInvites.ContainsKey(inviterPlayer.Id))
            {
                player.SendPacket("0|A|STM|msg_grp_inv_err_inviter_nonexistant");
                return;
            }
            if (inviterPlayer.Group == null)
            {
                new Group(inviterPlayer, player);
            }
            else if (inviterPlayer.Group.Members.Count < Group.DEFAULT_MAX_GROUP_SIZE)
            {
                inviterPlayer.Group.Accept(inviterPlayer, player);
            }
            player.Storage.GroupInvites.Clear();
        }
    }
}
