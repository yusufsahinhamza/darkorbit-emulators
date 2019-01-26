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
    class SelectRocketRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new SelectRocketRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var settingsManager = player.SettingsManager;
            var newSelectedRocket = read.rocketType.typeValue;

            if (settingsManager.SelectedRocket == newSelectedRocket)
            {
                if (player.Settings.Gameplay.quickSlotStopAttack)
                {
                    player.AttackManager.RocketAttack();
                }
            }
            else
            {
                settingsManager.SelectedRocket = newSelectedRocket;
                player.Settings.ShipSettings.selectedRocket = newSelectedRocket;
                QueryManager.SavePlayer.Settings(player);
            }
        }
    }
}
