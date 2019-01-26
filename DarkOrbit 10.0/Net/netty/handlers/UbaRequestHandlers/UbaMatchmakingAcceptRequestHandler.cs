using Ow.Game;
using Ow.Game.Events;
using Ow.Game.Movements;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers.UbaRequestHandlers
{
    class UbaMatchmakingAcceptRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;
            player.SendCommand(UbaWindowInitializationCommand.write(new class_NQ(), 4));
            player.UbaMatchmakingAccepted = true;

            if (player.UbaOpponent.UbaMatchmakingAccepted)
            {
                var position1 = new Position(4400, 3600);
                var position2 = new Position(5600, 2400);

                player.Jump(121, position1);
                player.UbaOpponent.Jump(121, position2);

                player.SendCommand(UbaWindowInitializationCommand.write(new class_NQ(), 5));
                player.UbaOpponent.SendCommand(UbaWindowInitializationCommand.write(new class_NQ(), 5));

                new Uba(player, player.UbaOpponent);
            }
        }
    }
}
