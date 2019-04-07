using Ow.Game;
using Ow.Game.Objects.Stations;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests.BattleStationRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers.BattleStationRequestHandlers
{
    class EquipModuleRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new EquipModuleRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var activatableStationaryMapEntity = player.Spacemap.GetActivatableMapEntity(read.battleStationId);

            if (activatableStationaryMapEntity != null && activatableStationaryMapEntity is BattleStation battleStation)
            {
                if (player.Position.DistanceTo(activatableStationaryMapEntity.Position) > 700)
                {
                    player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.OUT_OF_RANGE));
                    return;
                }

                /*
                if (!battleStation.ModuleInstallationState)
                {
                    player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.CONCURRENT_EQUIP));
                    return;
                }
                */
                var module = battleStation.inventoryStationModule.Where(x => x.itemId == read.itemId).First();

                if (module.itemId == BattleStation.DEFLECTOR_ID && read.slotId != 1 || module.itemId != BattleStation.DEFLECTOR_ID && read.slotId == 1) return;
                if (module.itemId == BattleStation.HULL_ID && read.slotId != 0 || module.itemId != BattleStation.HULL_ID && read.slotId == 0) return;

                battleStation.inventoryStationModule.Remove(module);
                module.slotId = read.slotId;
                //module.installationSecondsLeft = 60;
                //battleStation.ModuleInstallationSeconds = DateTime.Now;
                battleStation.equippedStationModule.Add(module);
                battleStation.Click(gameSession);
            }
        }
    }
}
