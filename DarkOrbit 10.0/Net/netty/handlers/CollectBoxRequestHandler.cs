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
    class CollectBoxRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new CollectBoxRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;

            var collectable = player.Spacemap.Collectables.Values.Where(x => x.Hash == read.hash).FirstOrDefault();
            if (collectable != null)
                collectable.Collect(player);
        }
    }
}
