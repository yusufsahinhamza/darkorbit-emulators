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
    class UnEquipModuleRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new UnEquipModuleRequest();
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

                var module = battleStation.equippedStationModule.Where(x => x.itemId == read.itemId).First();
                battleStation.equippedStationModule.Remove(module);
                battleStation.inventoryStationModule.Add(module);
                battleStation.Click(gameSession);
            }
        }
    }
}
