using Ow.Game;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class ReadyRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new ReadyRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;

            switch (read.readyType)
            {
                case ReadyRequest.MAP_LOADED:
                    
                    break;
                case ReadyRequest.HERO_LOADED:
                    
                    break;
            }
        }
    }
}
