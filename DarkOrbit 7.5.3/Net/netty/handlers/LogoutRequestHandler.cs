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
    class LogoutRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new LogoutRequest();
            read.readCommand(bytes);

            switch (read.request)
            {
                case LogoutRequest.REQUEST_LOGOUT:
                    gameSession.Player.Logout(true);
                    break;
                case LogoutRequest.ABORT_LOGOUT:
                    gameSession.Player.AbortLogout();
                    break;

            }
        }
    }
}
