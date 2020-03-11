using Ow.Game;
using Ow.Game.Movements;
using Ow.Game.Objects;
using Ow.Game.Objects.Stations;
using Ow.Managers;
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
            var battleStation = player.Spacemap.GetActivatableMapEntity(read.battleStationId) as BattleStation;

            if (battleStation != null)
            {
                if (player.Position.DistanceTo(battleStation.Position) > 700)
                {
                    player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.OUT_OF_RANGE));
                    return;
                }

                if (!battleStation.EquippedStationModule.ContainsKey(player.Clan.Id))
                    battleStation.EquippedStationModule[player.Clan.Id] = new List<Satellite>();
                else
                {
                    if (battleStation.EquippedStationModule[player.Clan.Id].Count >= 10) return;
                }

                if (battleStation.EquippedStationModule[player.Clan.Id].Where(x => !x.Installed && x.OwnerId == player.Id).ToList().Count > 0)
                {
                    player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.EQUIP_OF_SAME_PLAYER_RUNNING));
                    return;
                }

                var module = player.Storage.BattleStationModules.Where(x => x.Id == read.itemId).FirstOrDefault();

                if (module != null)
                {
                    if (module.Type == StationModuleModule.DEFLECTOR && read.slotId != 1 || module.Type != StationModuleModule.DEFLECTOR && read.slotId == 1) return;
                    if (module.Type == StationModuleModule.HULL && read.slotId != 0 || module.Type != StationModuleModule.HULL && read.slotId == 0) return;
                    if ((module.Type == StationModuleModule.DEFLECTOR || module.Type == StationModuleModule.HULL || module.Type == StationModuleModule.REPAIR || module.Type == StationModuleModule.DAMAGE_BOOSTER || module.Type == StationModuleModule.EXPERIENCE_BOOSTER || module.Type == StationModuleModule.HONOR_BOOSTER) && battleStation.EquippedStationModule[player.Clan.Id].Where(x => x.Type == module.Type).Count() >= 1) return;

                    var equippedModule = battleStation.EquippedStationModule[player.Clan.Id].Where(x => x.SlotId == read.slotId).FirstOrDefault();

                    if (read.replace || equippedModule != null)
                    {
                        if (read.replace && equippedModule.OwnerId != player.Id)
                        {
                            player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.ITEM_NOT_OWNED));
                            return;
                        }
                        else if (!read.replace && !equippedModule.Installed && equippedModule.OwnerId != player.Id)
                        {
                            player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.CONCURRENT_EQUIP));
                            return;
                        }

                        equippedModule.Remove(false, false);

                        if (battleStation.AssetTypeId == AssetTypeModule.BATTLESTATION)
                        {
                            equippedModule.Spacemap.Activatables.TryRemove(equippedModule.Id, out var activatable);
                            GameManager.SendCommandToMap(equippedModule.Spacemap.Id, AssetRemoveCommand.write(equippedModule.GetAssetType(), equippedModule.Id));
                        }
                    }

                    int designId = module.Type == StationModuleModule.REPAIR ? 3 : module.Type == StationModuleModule.LASER_HIGH_RANGE ? 4 : module.Type == StationModuleModule.LASER_MID_RANGE ? 5 : module.Type == StationModuleModule.LASER_LOW_RANGE ? 6 : module.Type == StationModuleModule.ROCKET_LOW_ACCURACY ? 8 : module.Type == StationModuleModule.ROCKET_MID_ACCURACY ? 7 : module.Type == StationModuleModule.HONOR_BOOSTER ? 9 : module.Type == StationModuleModule.DAMAGE_BOOSTER ? 10 : module.Type == StationModuleModule.EXPERIENCE_BOOSTER ? 11 : 0;

                    var satellite = new Satellite(battleStation, player.Id, Satellite.GetName(module.Type), designId, module.Id, read.slotId, module.Type, Satellite.GetPosition(battleStation.Position, read.slotId));
                    satellite.InstallationSecondsLeft = battleStation.AssetTypeId == AssetTypeModule.BATTLESTATION ? 0 : 0; //0 is for dont wait for install modules, if you want to wait for install modules change it, also there is different wait times for installing modules to already owned battlestation by a clan or just satellite e.g. 240 : 60

                    module.InUse = true;

                    battleStation.EquippedStationModule[player.Clan.Id].Add(satellite);

                    if (battleStation.AssetTypeId == AssetTypeModule.BATTLESTATION)
                    {
                        satellite.AddVisualModifier(VisualModifierCommand.BATTLESTATION_INSTALLING, 0, "", 0, true);
                        satellite.Spacemap.Activatables.TryAdd(satellite.Id, satellite);

                        foreach (var character in satellite.Spacemap.Characters.Values)
                        {
                            if (character is Player)
                            {
                                short relationType = character.Clan.Id != 0 && satellite.Clan.Id != 0 ? satellite.Clan.GetRelation(character.Clan) : (short)0;
                                (character as Player).SendCommand(satellite.GetAssetCreateCommand(relationType));
                            }
                        }
                    }

                    battleStation.Click(gameSession);

                    QueryManager.SavePlayer.Modules(player);
                    QueryManager.BattleStations.Modules(battleStation);

                    //send command to other players
                }
            }
        }
    }
}
