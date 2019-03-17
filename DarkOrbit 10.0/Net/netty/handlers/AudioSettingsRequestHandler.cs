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
    class AudioSettingsRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new AudioSettingsRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var audioSettings = player.Settings.Audio;

            audioSettings.notSet = false;
            audioSettings.music = read.music;
            audioSettings.sound = read.sound;
            audioSettings.voice = read.voice;
            audioSettings.playCombatMusic = read.playCombatMusic;

            QueryManager.SavePlayer.Settings(player, "audio", audioSettings);
        }
    }
}
