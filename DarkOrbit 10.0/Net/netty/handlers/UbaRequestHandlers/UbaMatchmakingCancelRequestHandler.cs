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
            var player = gameSession.Player;
            player.SendCommand(UbaWindowInitializationCommand.write(new class_NQ(), 0));

            if(player.UbaOpponent != null)
            {
                player.UbaOpponent.SendCommand(UbaWindowInitializationCommand.write(new class_NQ(), 0));
                player.UbaOpponent.UbaOpponent = null;
            }

            player.UbaOpponent = null;
            EventManager.UltimateBattleArena.RemovePlayer(player);
        }
    }
}
