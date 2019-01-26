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
    class HellstormLaunchRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;
            var rocketLauncher = player.AttackManager.RocketLauncher;
            player.AttackManager.LaunchRocketLauncher();
            rocketLauncher.ReloadingActive = player.AutoRocketLauncher ? true : false;
        }
    }
}
