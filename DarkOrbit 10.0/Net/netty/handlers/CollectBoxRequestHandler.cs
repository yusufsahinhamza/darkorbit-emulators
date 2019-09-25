using Ow.Game;
using Ow.Game.Movements;
using Ow.Game.Objects;
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

            var obj = player.Spacemap.Objects.Values.Where(x => x is Collectable collectable && collectable.Hash == read.hash).FirstOrDefault();
            if (obj != null)
                (obj as Collectable).Collect(player);
        }
    }
}
