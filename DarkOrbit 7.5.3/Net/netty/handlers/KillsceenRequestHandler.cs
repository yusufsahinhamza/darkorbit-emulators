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
    class KillsceenRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new KillscreenRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;

            switch(read.selection.repairTypeValue)
            {
                case KillScreenOptionTypeModule.BASIC_REPAIR:
                    player.Respawn(true);
                    break;
                case KillScreenOptionTypeModule.BASIC_FULL_REPAIR:
                    player.Respawn(true, false, false, true);
                    break;
                case KillScreenOptionTypeModule.AT_DEATHLOCATION_REPAIR:
                    player.Respawn(false, true, false);
                    break;
                case KillScreenOptionTypeModule.AT_JUMPGATE_REPAIR:
                    player.Respawn(false, false, true);
                    break;
            }
        }
    }
}
