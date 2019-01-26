using Ow.Game;
using Ow.Game.Objects.Players.Managers;
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
    class HellstormSelectRocketRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new HellstormSelectRocketRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var settingsManager = player.SettingsManager;
            var newSelectedRocketLauncher = read.rocketType.typeValue;
            
            if (settingsManager.SelectedRocketLauncher != newSelectedRocketLauncher)
            {
                settingsManager.SelectedRocketLauncher = newSelectedRocketLauncher;
                player.Settings.ShipSettings.selectedRocketLauncher = newSelectedRocketLauncher;
                player.AttackManager.RocketLauncher.ChangeLoad(settingsManager.SelectedRocketLauncher);

                QueryManager.SavePlayer.Settings(player);
            }
            
        }
    }
}
