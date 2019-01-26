using Ow.Game;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class UIOpenRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new UIOpenRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            switch (read.itemId)
            {
                case UIOpenRequest.ACTION_LOGOUT:
                    player.Logout(true);
                    break;
                case UIOpenRequest.ACTION_SHIP_WARP:
                    //gemi değiştirme ekranı gönder
                    break;
            }
        }
    }
}
