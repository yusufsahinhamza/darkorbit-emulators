using Ow.Game;
using Ow.Game.Movements;
using Ow.Game.Objects.Stations;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class EquipModuleRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new EquipModuleRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;

            var activatableStationaryMapEntity = player.Spacemap.GetActivatableMapEntity(read.battleStationId);

            if (activatableStationaryMapEntity != null)
                if (activatableStationaryMapEntity is BattleStation)
                {
                    var battleStation = activatableStationaryMapEntity as BattleStation;
                    //var asdas = new StationModuleModule(battleStation.GetAssetId(), read.itemId, read.slotId, StationModuleModule.HONOR_BOOSTER, 10000, 10000, 1000, 1000, 16, "TESTTASDT", 0, 0, 0, 0, 0);
                    //battleStation.stationModuleModules1.Add(asdas);
                    //player.SendCommand(BattleStationStatusCommand.write(battleStation.GetAssetId(), battleStation.GetAssetId(), "Battle Station", true, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, new EquippedModulesModule(battleStation.stationModuleModules1), true));
                }

        }
    }
}
