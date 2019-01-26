using Ow.Game;
using Ow.Game.Objects;
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
    class HellstormLoadRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;
            var rocketLauncher = player.AttackManager.RocketLauncher;

            if (rocketLauncher.CurrentLoad != rocketLauncher.MaxLoad)
                rocketLauncher.Reload();
            else
                player.AttackManager.LaunchRocketLauncher();
        }
    }
}
