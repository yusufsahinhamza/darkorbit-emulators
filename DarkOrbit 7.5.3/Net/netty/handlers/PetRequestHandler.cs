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
    class PetRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new PetRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            if (player.RankId != 21) return;

            switch (read.petRequestType)
            {
                case PetRequest.LAUNCH:
                    player.Pet.Activate();
                    break;
                case PetRequest.TOGGLE_ACTIVATION:
                    player.Pet.Activate();
                    break;
                case PetRequest.DEACTIVATE:
                    player.Pet.Deactivate();
                    break;
                case PetRequest.HOTKEY_GUARD_MODE:
                    player.Pet.SwitchGear(PetGearTypeModule.GUARD);
                    break;
                case PetRequest.HOTKEY_REPAIR_SHIP:
                    //player.Pet.ComboShipRepair();
                    break;
            }
        }
    }
}
