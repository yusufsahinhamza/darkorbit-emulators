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
    class UbaMatchmakingRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;

            if (player.Spacemap.Id != EventManager.JackpotBattle.Spacemap.Id)
            {
                player.SendCommand(UbaWindowInitializationCommand.write(new class_NQ(), 2));
                EventManager.UltimateBattleArena.AddPlayer(player);
            }
        }
    }
}
