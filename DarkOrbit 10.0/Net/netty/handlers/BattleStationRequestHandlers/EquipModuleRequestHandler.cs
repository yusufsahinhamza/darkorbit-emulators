using Ow.Game;
using Ow.Game.Objects.Stations;
using Ow.Game.Objects.Stations.BattleStations;
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

                if (battleStation.EquippedStationModule.ContainsKey(player.Clan.Id))
                {
                    if (battleStation.EquippedStationModule[player.Clan.Id].Where(x => !x.Installed && x.OwnerId == player.Id).ToList().Count > 0)
                    {
                        player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.EQUIP_OF_SAME_PLAYER_RUNNING));
                        return;
                    }

                    var equippedModule = battleStation.EquippedStationModule[player.Clan.Id].Where(x => x.ModuleModule.slotId == read.slotId).FirstOrDefault();

                    if (equippedModule != null)
                    {
                        if (equippedModule.OwnerId != player.Id)
                        {
                            player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.ITEM_NOT_OWNED));
                            return;
                        }

                        battleStation.EquippedStationModule[player.Clan.Id].Remove(equippedModule);
                    }
                }

                var module = player.Storage.BattleStationModules.Where(x => x.itemId == read.itemId).FirstOrDefault();

                if (module.itemId == BattleStation.DEFLECTOR_ID && read.slotId != 1 || module.itemId != BattleStation.DEFLECTOR_ID && read.slotId == 1) return;
                if (module.itemId == BattleStation.HULL_ID && read.slotId != 0 || module.itemId != BattleStation.HULL_ID && read.slotId == 0) return;

                module.slotId = read.slotId;
                module.installationSecondsLeft = module.installationSeconds;
                
                if (!battleStation.EquippedStationModule.ContainsKey(player.Clan.Id))
                    battleStation.EquippedStationModule[player.Clan.Id] = new List<Module>();

                battleStation.EquippedStationModule[player.Clan.Id].Add(new Module(battleStation, module, player.Id));
                battleStation.Click(gameSession);
                //send command to other players
            }
        }
    }
}
