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
    class SelectBatteryRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new SelectBatteryRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var settingsManager = player.SettingsManager;
            var newSelectedLaser = read.batteryType.typeValue;

            if (settingsManager.SelectedLaser == newSelectedLaser)
            {
                if (player.Settings.Gameplay.quickSlotStopAttack)
                {
                    if (player.Selected != null)
                    {
                        if (player.AttackManager.Attacking)
                            player.DisableAttack(newSelectedLaser);
                        else
                            player.EnableAttack(newSelectedLaser);
                    }
                }
            }
            else
            {            
                settingsManager.SelectedLaser = newSelectedLaser;
                player.Settings.ShipSettings.selectedLaser = newSelectedLaser;
               
                QueryManager.SavePlayer.Settings(player);
            }
        }
    }
}
