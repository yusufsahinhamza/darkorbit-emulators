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
    class AttackAbortLaserRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var player = gameSession.Player;

            if(player.Selected != null)
            {
                player.DisableAttack(player.Settings.InGameSettings.selectedLaser);
                player.SendPacket("0|A|STM|attstop|%!|" + (EventManager.JackpotBattle.Active && player.Spacemap.Id == EventManager.JackpotBattle.Spacemap.Id ? EventManager.JackpotBattle.Name : player.SelectedCharacter.Name));
            }
        }
    }
}
