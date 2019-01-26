using Ow.Game;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ow.Game.Objects.Players.Managers.PlayerSettings;

namespace Ow.Net.netty.handlers
{
    class UserKeyBindingsUpdateHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {                
            var read = new UserKeyBindingsUpdateRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var keys = player.Settings.BoundKeys;
            keys.Clear();

            foreach (var action in read.changedKeyBindings)
            {
                keys.Add(new BoundKeysBase(action.actionType, (short)action.charCode, action.parameter, action.keyCodes));
            }

            QueryManager.SavePlayer.Settings(player);          
        }
    }
}
