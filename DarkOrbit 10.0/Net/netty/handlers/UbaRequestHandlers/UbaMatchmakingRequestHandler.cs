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
            if (EventManager.JackpotBattle.InEvent(player) || Duel.InDuel(player)) return;
            EventManager.UltimateBattleArena.AddWaitingPlayer(player);
        }
    }
}
