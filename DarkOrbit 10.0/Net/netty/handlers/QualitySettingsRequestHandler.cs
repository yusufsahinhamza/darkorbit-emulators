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
    class QualitySettingsRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new QualitySettingsRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var qualitySettings = player.Settings.Quality;

            qualitySettings.notSet = false;
            qualitySettings.qualityAttack = read.qualityAttack;
            qualitySettings.qualityBackground = read.qualityBackground;
            qualitySettings.qualityCollectable = read.qualityCollectable;
            qualitySettings.qualityCustomized = read.qualityCustomized;
            qualitySettings.qualityEffect = read.qualityEffect;
            qualitySettings.qualityEngine = read.qualityEngine;
            qualitySettings.qualityExplosion = read.qualityExplosion;
            qualitySettings.qualityPoizone = read.qualityPoizone;
            qualitySettings.qualityPresetting = read.qualityPresetting;
            qualitySettings.qualityShip = read.qualityShip;

            QueryManager.SavePlayer.Settings(player, "quality", qualitySettings);
        }
    }
}
