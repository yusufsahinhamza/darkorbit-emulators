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
    class GroupUpdateBlockInvitationStateRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;

            player.Settings.InGameSettings.BlockedGroupInvites = player.Settings.InGameSettings.BlockedGroupInvites ? false : true;
            player.SendCommand(GroupUpdateBlockInvitationState.write(player.Settings.InGameSettings.BlockedGroupInvites));

            QueryManager.SavePlayer.Settings(player);
        }
    }
}
