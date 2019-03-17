using Ow.Game;
using Ow.Game.Events;
using Ow.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers.UbaRequestHandlers
{
    class UbaMatchmakingCancelRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            /*
            var player = gameSession.Player;
            player.SendCommand(UbaWindowInitializationCommand.write(new command_NQ(), 0));

            if(player.Storage.UbaOpponent != null)
            {
                player.Storage.UbaOpponent.SendCommand(UbaWindowInitializationCommand.write(new command_NQ(), 0));
                player.Storage.UbaOpponent.Storage.UbaOpponent = null;
            }

            player.Storage.UbaOpponent = null;
            EventManager.UltimateBattleArena.RemovePlayer(player);
            */
        }
    }
}
