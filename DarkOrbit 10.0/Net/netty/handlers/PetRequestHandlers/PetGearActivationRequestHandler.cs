using Ow.Game;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers.PetRequestHandlers
{
    class PetGearActivationRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new PetGearActivationRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            if (player.Pet == null) return;

            player.Pet.SwitchGear(read.gearId);
        }
    }
}
