using Ow.Game;
using Ow.Game.Movements;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class MoveRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new MoveRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var position = new Position(read.targetX, read.targetY);
            Movement.Move(player, position);
        }
    }
}
