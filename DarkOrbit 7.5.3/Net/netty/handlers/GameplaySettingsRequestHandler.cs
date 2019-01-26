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
    class GameplaySettingsRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new GameplaySettingsRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var gameplaySettings = player.Settings.Gameplay;

            gameplaySettings.notSet = false;
            gameplaySettings.autoBoost = read.autoBoost;
            gameplaySettings.autoBuyBootyKeys = read.autoBuyGreenBootyKeys;
            gameplaySettings.autoChangeAmmo = read.autoChangeAmmo;
            gameplaySettings.autoRefinement = read.autoRefinement;
            gameplaySettings.autoStartEnabled = read.autoStart;
            gameplaySettings.doubleclickAttackEnabled = read.doubleclickAttack;
            gameplaySettings.quickSlotStopAttack = read.quickslotStopAttack;

            QueryManager.SavePlayer.Settings(player);
        }
    }
}
