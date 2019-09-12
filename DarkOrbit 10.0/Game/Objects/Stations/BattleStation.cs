using Ow.Game;
using Ow.Game.Movements;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Stations
{
    class BattleStation : Activatable
    {
        public Dictionary<int, List<Satellite>> EquippedStationModule = new Dictionary<int, List<Satellite>>();

        public override string Name => Clan.Id != 0 ? Clan.Name : "Julius";

        public override short AssetTypeId => Clan.Id == 0 || buildTime.AddMinutes(BuildTimeInMinutes) > DateTime.Now? AssetTypeModule.ASTEROID : AssetTypeModule.BATTLESTATION;

        public bool InBuildingState = false;
        public int BuildTimeInMinutes = 0;

        public BattleStation(Spacemap spacemap, int factionId, Position position, Clan clan) : base(spacemap, factionId, position, clan)
        {
            ShieldAbsorption = 0.8;
            CurrentHitPoints = 250000;
            MaxHitPoints = 500000;
            CurrentShieldPoints = 250000;
            MaxShieldPoints = 500000;

            var tickId = -1;
            TickId = tickId;
        }

        public DateTime buildTime = new DateTime();
        public new void Tick()
        {
            if (InBuildingState && buildTime.AddMinutes(BuildTimeInMinutes) < DateTime.Now)
            {
                RemoveVisualModifier(VisualModifierCommand.BATTLESTATION_CONSTRUCTING);
                //Visuals.Add(new VisualModifierCommand(Id, VisualModifierCommand.BATTLESTATION_DOWNTIME_TIMER, 1800, "", 0, true));
                PrepareSatellites();

                GameManager.SendCommandToMap(Spacemap.Id, AssetRemoveCommand.write(GetAssetType(), Id));

                foreach (var character in Spacemap.Characters.Values)
                {
                    if (character is Player player)
                    {
                        short relationType = character.Clan.Id != 0 && Clan.Id != 0 ? Clan.GetRelation(character.Clan) : (short)0;
                        player.SendCommand(GetAssetCreateCommand(relationType));
                    }
                }

                BuildTimeInMinutes = 0;
                InBuildingState = false;
            }
        }

        public void UpdatePlayerModuleInventory(Player player)
        {
            player.Storage.BattleStationModules.Clear();
            
            for (var i = 1; i <= 8; i++)
                player.Storage.BattleStationModules.Add(new StationModuleModule(Id, i, i, StationModuleModule.LASER_HIGH_RANGE, 1, 1, 1, 1, 16, player.Name, 0, 0 , 0, 0, 500));
            for (var i = 9; i <= 16; i++)
                player.Storage.BattleStationModules.Add(new StationModuleModule(Id, i, i, StationModuleModule.ROCKET_LOW_ACCURACY, 1, 1, 1, 1, 16, player.Name, 0, 0, 0, 0, 500));
            for (var i = 15; i <= 22; i++)
                player.Storage.BattleStationModules.Add(new StationModuleModule(Id, i, i, StationModuleModule.ROCKET_MID_ACCURACY, 1, 1, 1, 1, 16, player.Name, 0, 0, 0, 0, 500));

            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, 23, 23, StationModuleModule.DAMAGE_BOOSTER, 1, 1, 1, 1, 16, player.Name, 0, 0, 0, 0, 500));
            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, 24, 24, StationModuleModule.EXPERIENCE_BOOSTER, 1, 1, 1, 1, 16, player.Name, 0, 0, 0, 0, 500));
            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, 25, 25, StationModuleModule.HONOR_BOOSTER, 1, 1, 1, 1, 16, player.Name, 0, 0, 0, 0, 500));
            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, 26, 26, StationModuleModule.REPAIR, 1, 1, 1, 1, 16, player.Name, 0, 0, 0, 0, 500));
            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, 27, 27, StationModuleModule.DEFLECTOR, 1, 1, 1, 1, 16, player.Name, 0, 0, 0, 0, 500));
            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, 28, 28, StationModuleModule.HULL, 1, 1, 1, 1, 16, player.Name, 0, 0, 0, 0, 500));

            foreach (var station in Spacemap.Activatables.Values)
            {
                if (station is BattleStation battleStation)
                {
                    if (battleStation.EquippedStationModule.ContainsKey(player.Clan.Id))
                    {
                        foreach (var module in battleStation.EquippedStationModule[player.Clan.Id])
                        {
                            if (module.OwnerId == player.Id)
                            {
                                var playerModule = player.Storage.BattleStationModules.Where(x => x.itemId == module.Module.itemId).FirstOrDefault();
                                player.Storage.BattleStationModules.Remove(playerModule);
                            }
                        }
                    }
                }
            }
        }

        public void PrepareSatellites()
        {
            foreach (var satellite in EquippedStationModule[Clan.Id])
            {
                if (satellite.Module.type != StationModuleModule.DEFLECTOR && satellite.Module.type != StationModuleModule.HULL)
                {
                    foreach (var character in satellite.Spacemap.Characters.Values)
                    {
                        if (character is Player player)
                        {
                            Spacemap.Activatables.TryAdd(satellite.Id, satellite);

                            short relationType = character.Clan.Id != 0 && satellite.Clan.Id != 0 ? satellite.Clan.GetRelation(character.Clan) : (short)0;
                            player.SendCommand(satellite.GetAssetCreateCommand(relationType));
                        }
                    }
                }
            }
        }

        public override void Click(GameSession gameSession)
        {
            var player = gameSession.Player;
            UpdatePlayerModuleInventory(player);

            int secondsLeft = (int)(TimeSpan.FromMinutes(BuildTimeInMinutes).TotalSeconds - (DateTime.Now - buildTime).TotalSeconds);

            if (InBuildingState)
                player.SendCommand(BattleStationBuildingStateCommand.write(Id, Id, Name, secondsLeft, 0, Clan.Name, new FactionModule((short)FactionId)));
            else
            {
                if (Clan.Id == 0 || buildTime.AddMinutes(BuildTimeInMinutes) > DateTime.Now)
                {
                    var bestClan = EquippedStationModule.Values.Where(x => x.Where(y => y.Installed).ToList().Count() > 0).ToList().Count > 0 ? EquippedStationModule.OrderByDescending(x => x.Value.Count) : null;
                    player.SendCommand(BattleStationBuildingUiInitializationCommand.write(Id, Id, Name,
                                      new AsteroidProgressCommand(
                                              Id,
                                              (float)(EquippedStationModule.ContainsKey(player.Clan.Id) ? EquippedStationModule[player.Clan.Id].Where(x => x.Installed).ToList().Count : 0) / 10,
                                              (float)(bestClan != null ? bestClan.FirstOrDefault().Value.Where(x => x.Installed).ToList().Count : 0) / 10,
                                              player.Clan.Name,
                                              bestClan != null ? GameManager.GetClan(bestClan.FirstOrDefault().Key).Name :  "Leading clan's progress",                                             
                                              new EquippedModulesModule(EquippedStationModule.ContainsKey(player.Clan.Id) ? EquippedStationModule[player.Clan.Id].Select(x => x.Module).ToList() : new List<StationModuleModule>()),
                                              (EquippedStationModule.ContainsKey(player.Clan.Id) ? EquippedStationModule[player.Clan.Id].Where(x => x.Installed).ToList().Count : 0) == 10),
                                      new AvailableModulesCommand(player.Storage.BattleStationModules),
                                      1,
                                      60,
                                      0));
                }
                else
                {
                    if (player.Clan.Id == Clan.Id)
                        player.SendCommand(BattleStationStatusCommand.write(Id, Id, Name, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new EquippedModulesModule(EquippedStationModule[player.Clan.Id].Select(x => x.Module).ToList()), false));
                }
            }
        }

        public override byte[] GetAssetCreateCommand(short clanRelationModule = ClanRelationModule.NONE)
        {
            return AssetCreateCommand.write(GetAssetType(), Name,
                                          FactionId, Clan.Tag, Id, 0, 0,
                                          Position.X, Position.Y, Clan.Id, true, true, true, true,
                                          new ClanRelationModule(clanRelationModule),
                                          VisualModifiers.Values.ToList());
        }
    }
}
