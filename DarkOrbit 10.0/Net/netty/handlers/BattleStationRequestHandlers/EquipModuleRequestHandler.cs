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

                if (battleStation.EquippedStationModule[player.Clan.Id].Where(x => !x.Installed && x.OwnerId == player.Id).ToList().Count > 0)
                {
                    player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.EQUIP_OF_SAME_PLAYER_RUNNING));
                    return;
                }

                var equippedModule = battleStation.EquippedStationModule[player.Clan.Id].Where(x => x.Module.slotId == read.slotId).FirstOrDefault();

                if (read.replace || equippedModule != null)
                {
                    if (equippedModule.OwnerId != player.Id && read.replace)
                    {
                        player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.ITEM_NOT_OWNED));
                        return;
                    }

                    battleStation.EquippedStationModule[player.Clan.Id].Remove(equippedModule);

                    if (!read.replace)
                    {
                        Activatable activatable;
                        equippedModule.Spacemap.Activatables.TryRemove(equippedModule.Id, out activatable);
                        GameManager.SendCommandToMap(equippedModule.Spacemap.Id, AssetRemoveCommand.write(equippedModule.GetAssetType(), equippedModule.Id));
                    }
                }

                var module = player.Storage.BattleStationModules.Where(x => x.itemId == read.itemId).FirstOrDefault();

                if (module != null)
                {
                    if (module.type == StationModuleModule.DEFLECTOR && read.slotId != 1 || module.type != StationModuleModule.DEFLECTOR && read.slotId == 1) return;
                    if (module.type == StationModuleModule.HULL && read.slotId != 0 || module.type != StationModuleModule.HULL && read.slotId == 0) return;
                    if ((module.type == StationModuleModule.DEFLECTOR || module.type == StationModuleModule.HULL || module.type == StationModuleModule.REPAIR || module.type == StationModuleModule.DAMAGE_BOOSTER || module.type == StationModuleModule.EXPERIENCE_BOOSTER || module.type == StationModuleModule.HONOR_BOOSTER) && battleStation.EquippedStationModule[player.Clan.Id].Where(x => x.Module.type == module.type).Count() >= 1) return;

                    module.slotId = read.slotId;
                    module.installationSecondsLeft = (!read.replace && equippedModule != null) ? 60 : 1;

                    var center = battleStation.Position;
                    int designId = module.type == StationModuleModule.REPAIR ? 3 : module.type == StationModuleModule.LASER_HIGH_RANGE ? 4 : module.type == StationModuleModule.LASER_MID_RANGE ? 5 : module.type == StationModuleModule.LASER_LOW_RANGE ? 6 : module.type == StationModuleModule.ROCKET_LOW_ACCURACY ? 7 : module.type == StationModuleModule.ROCKET_MID_ACCURACY ? 8 : module.type == StationModuleModule.HONOR_BOOSTER ? 9 : module.type == StationModuleModule.DAMAGE_BOOSTER ? 10 : module.type == StationModuleModule.EXPERIENCE_BOOSTER ? 11 : 0;
                    string name = module.type == StationModuleModule.REPAIR ? "REPM-1" : module.type == StationModuleModule.LASER_HIGH_RANGE ? "LTM-HR" : module.type == StationModuleModule.LASER_MID_RANGE ? "LTM-MR" : module.type == StationModuleModule.LASER_LOW_RANGE ? "LTM-LR" : module.type == StationModuleModule.ROCKET_LOW_ACCURACY ? "RAM-LA" : module.type == StationModuleModule.ROCKET_MID_ACCURACY ? "RAM-MA" : module.type == StationModuleModule.HONOR_BOOSTER ? "HONM-1" : module.type == StationModuleModule.DAMAGE_BOOSTER ? "DMGM-1" : module.type == StationModuleModule.EXPERIENCE_BOOSTER ? "XPM-1" : "";
                    var position = module.slotId == 9 ? new Position(center.X - 171, center.Y - 236) : module.slotId == 2 ? new Position(center.X + 170, center.Y - 235) : module.slotId == 3 ? new Position(center.X + 412, center.Y - 98) : module.slotId == 4 ? new Position(center.X + 412, center.Y + 97) : module.slotId == 5 ? new Position(center.X + 170, center.Y + 236) : module.slotId == 6 ? new Position(center.X - 171, center.Y + 235) : module.slotId == 7 ? new Position(center.X - 413, center.Y + 97) : module.slotId == 8 ? new Position(center.X - 413, center.Y - 98) : null;

                    var satellite = new Satellite(battleStation, module, player.Id, name, designId, position);

                    if (!read.replace && equippedModule != null)
                    {
                        satellite.AddVisualModifier(new VisualModifierCommand(satellite.Id, VisualModifierCommand.BATTLESTATION_INSTALLING, 0, "", 0, true));

                        foreach (var character in satellite.Spacemap.Characters.Values)
                        {
                            if (character is Player)
                            {
                                satellite.Spacemap.Activatables.TryAdd(satellite.Id, satellite);

                                short relationType = character.Clan.Id != 0 && satellite.Clan.Id != 0 ? satellite.Clan.GetRelation(character.Clan) : (short)0;
                                (character as Player).SendCommand(satellite.GetAssetCreateCommand(relationType));
                            }
                        }
                    }

                    battleStation.EquippedStationModule[player.Clan.Id].Add(satellite);
                    battleStation.Click(gameSession);
                    //send command to other players
                }
            }
        }
    }
}
