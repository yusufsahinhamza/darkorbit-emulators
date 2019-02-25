using Ow.Game;
using Ow.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class AttackLaserRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;

            if (player.Selected != null)
                player.EnableAttack(player.Settings.InGameSettings.selectedLaser);
        }
    }
}
