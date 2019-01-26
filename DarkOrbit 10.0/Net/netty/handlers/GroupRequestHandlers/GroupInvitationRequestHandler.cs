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
    class GroupInvitationRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new GroupInvitationRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var invited = GetPlayerByName(read.name);

            if (invited != player)
            {
                if (invited == null)
                {
                    player.SendPacket("0|A|STM|msg_grp_inv_err_candidate_nonexistant");
                    return;
                }
                if (invited.Group != null)
                {
                    player.SendPacket("0|A|STM|msg_grp_inv_err_candidate_in_group");
                    return;
                }
                if (invited.Settings.InGameSettings.BlockedGroupInvites)
                {
                    player.SendPacket("0|A|STM|msg_grp_inv_err_candidate_blocking");
                    return;
                }
                if (invited.GroupInvites.ContainsKey(player.Id))
                {
                    player.SendPacket("0|A|STM|msg_grp_inv_err_duplicate_invitation");
                    return;
                }
                invited.GroupInvites.Add(player.Id, player.Group);

                player.SendCommand(GroupInviteCommand.write(player.Id, player.Name, new GroupPlayerShipModule(GroupPlayerShipModule.SENTINEL), invited.Id, invited.Name, new GroupPlayerShipModule(GroupPlayerShipModule.SENTINEL)));
                invited.SendCommand(GroupInviteCommand.write(player.Id, player.Name, new GroupPlayerShipModule(GroupPlayerShipModule.SENTINEL), invited.Id, invited.Name, new GroupPlayerShipModule(GroupPlayerShipModule.SENTINEL)));
            }
        }

        public Player GetPlayerByName(string name)
        {
            return GameManager.GameSessions.FirstOrDefault(x => x.Value.Player.Name == name).Value?.Player;
        }
    }
}
