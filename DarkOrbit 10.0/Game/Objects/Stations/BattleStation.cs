using Ow.Game;
using Ow.Game.Movements;
using Ow.Game.Objects.Stations.BattleStations;
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
    class BattleStation : Activatable, Tick
    {
        private static short ASSET_TYPE_CLAN_OWNING = AssetTypeModule.BATTLESTATION;
        private static short ASSET_TYPE_NO_CLAN = AssetTypeModule.ASTEROID;
        private static short DESIGN_ID = 0;

        public Dictionary<int, List<Module>> EquippedStationModule = new Dictionary<int, List<Module>>(); //change name as StationModules or not sure
        public List<VisualModifierCommand> Visuals = new List<VisualModifierCommand>();
        public List<Satellite> Modules = new List<Satellite>();

        public int TickId { get; set; }
        public string Name => Clan.Id != 0 ? Clan.Name : "Julius";
        public bool InBuildingState = false;
        public int BuildTimeInMinutes = 0;

        public BattleStation(Spacemap spacemap, int factionId, Position position, Clan clan) : base(spacemap, factionId, position, clan)
        {
            GameManager.BattleStations.TryAdd("Julius", this);
            var tickId = -1;
            TickId = tickId;
        }

        public DateTime buildTime = new DateTime();
        public void Tick()
        {
            if (InBuildingState && buildTime.AddMinutes(BuildTimeInMinutes) < DateTime.Now)
            {
                Visuals.Remove(Visuals.Where(x => x.modifier == VisualModifierCommand.BATTLESTATION_CONSTRUCTING).FirstOrDefault());
                //Visuals.Add(new VisualModifierCommand(Id, VisualModifierCommand.BATTLESTATION_DOWNTIME_TIMER, 1800, "", 0, true));
                PrepareSatellites();

                foreach (var character in Spacemap.Characters.Values)
                {
                    if (character is Player player)
                    {
                        short relationType = character.Clan.Id != 0 && Clan.Id != 0 ? Clan.GetRelation(character.Clan) : (short)0;
                        player.SendCommand(GetAssetCreateCommand(relationType));
                    }
                }

                InBuildingState = false;
            }
        }

        public static int DEFLECTOR_ID = 27;
        public static int HULL_ID = 28;

        public void UpdatePlayerModuleInventory(Player player)
        {
            player.Storage.BattleStationModules.Clear();
            
            for (var i = 1; i <= 8; i++)
                player.Storage.BattleStationModules.Add(new StationModuleModule(Id, i, i, StationModuleModule.LASER_HIGH_RANGE, 1, 1, 1, 1, 16, player.Name, 60, 0 , 0, 0, 500));
            for (var i = 9; i <= 16; i++)
                player.Storage.BattleStationModules.Add(new StationModuleModule(Id, i, i, StationModuleModule.ROCKET_LOW_ACCURACY, 1, 1, 1, 1, 16, player.Name, 60, 0, 0, 0, 500));
            for (var i = 15; i <= 22; i++)
                player.Storage.BattleStationModules.Add(new StationModuleModule(Id, i, i, StationModuleModule.ROCKET_MID_ACCURACY, 1, 1, 1, 1, 16, player.Name, 60, 0, 0, 0, 500));

            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, 23, 23, StationModuleModule.DAMAGE_BOOSTER, 1, 1, 1, 1, 16, player.Name, 60, 0, 0, 0, 500));
            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, 24, 24, StationModuleModule.EXPERIENCE_BOOSTER, 1, 1, 1, 1, 16, player.Name, 60, 0, 0, 0, 500));
            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, 25, 25, StationModuleModule.HONOR_BOOSTER, 1, 1, 1, 1, 16, player.Name, 60, 0, 0, 0, 500));
            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, 26, 26, StationModuleModule.REPAIR, 1, 1, 1, 1, 16, player.Name, 60, 0, 0, 0, 500));
            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, DEFLECTOR_ID, DEFLECTOR_ID, StationModuleModule.DEFLECTOR, 1, 1, 1, 1, 16, player.Name, 60, 0, 0, 0, 500));
            player.Storage.BattleStationModules.Add(new StationModuleModule(Id, HULL_ID, HULL_ID, StationModuleModule.HULL, 1, 1, 1, 1, 16, player.Name, 60, 0, 0, 0, 500));

            foreach (var station in GameManager.BattleStations.Values)
            {
                if (station.EquippedStationModule.ContainsKey(player.Clan.Id))
                {
                    foreach (var module in station.EquippedStationModule[player.Clan.Id])
                    {
                        if (module.OwnerId == player.Id)
                        {
                            var playerModule = player.Storage.BattleStationModules.Where(x => x.itemId == module.ModuleModule.itemId).FirstOrDefault();
                            player.Storage.BattleStationModules.Remove(playerModule);
                        }
                    }
                }
            }
        }

        public void PrepareSatellites()
        {
            foreach (var satellite in EquippedStationModule[Clan.Id])
            {
                if (satellite.ModuleModule.itemId == DEFLECTOR_ID || satellite.ModuleModule.itemId == HULL_ID) continue;

                var center = Position;
                int designId = satellite.ModuleModule.type == StationModuleModule.REPAIR ? 3 : satellite.ModuleModule.type == StationModuleModule.LASER_HIGH_RANGE ? 4 : satellite.ModuleModule.type == StationModuleModule.LASER_MID_RANGE ? 5 : satellite.ModuleModule.type == StationModuleModule.LASER_LOW_RANGE ? 6 : satellite.ModuleModule.type == StationModuleModule.ROCKET_LOW_ACCURACY ? 7 : satellite.ModuleModule.type == StationModuleModule.ROCKET_MID_ACCURACY ? 8 : satellite.ModuleModule.type == StationModuleModule.HONOR_BOOSTER ? 9 : satellite.ModuleModule.type == StationModuleModule.DAMAGE_BOOSTER ? 10 : satellite.ModuleModule.type == StationModuleModule.EXPERIENCE_BOOSTER ? 11 : 0;
                string name = satellite.ModuleModule.type == StationModuleModule.REPAIR ? "REPM-1" : satellite.ModuleModule.type == StationModuleModule.LASER_HIGH_RANGE ? "LTM-HR" : satellite.ModuleModule.type == StationModuleModule.LASER_MID_RANGE ? "LTM-MR" : satellite.ModuleModule.type == StationModuleModule.LASER_LOW_RANGE ? "LTM-LR" : satellite.ModuleModule.type == StationModuleModule.ROCKET_LOW_ACCURACY ? "RAM-LA" : satellite.ModuleModule.type == StationModuleModule.ROCKET_MID_ACCURACY ? "RAM-MA" : satellite.ModuleModule.type == StationModuleModule.HONOR_BOOSTER ? "HONM-1" : satellite.ModuleModule.type == StationModuleModule.DAMAGE_BOOSTER ? "DMGM-1" : satellite.ModuleModule.type == StationModuleModule.EXPERIENCE_BOOSTER ? "XPM-1" : "";
                var position = satellite.ModuleModule.slotId == 9 ? new Position(center.X - 171, center.Y - 236) : satellite.ModuleModule.slotId == 2 ? new Position(center.X + 170, center.Y - 235) : satellite.ModuleModule.slotId == 3 ? new Position(center.X + 412, center.Y - 98) : satellite.ModuleModule.slotId == 4 ? new Position(center.X + 412, center.Y + 97) : satellite.ModuleModule.slotId == 5 ? new Position(center.X + 170, center.Y + 236) : satellite.ModuleModule.slotId == 6 ? new Position(center.X - 171, center.Y + 235) : satellite.ModuleModule.slotId == 7 ? new Position(center.X - 413, center.Y + 97) : satellite.ModuleModule.slotId == 8 ? new Position(center.X - 413, center.Y - 98) : null;

                if (position != null)
                {
                    var module = new Satellite(Spacemap, name, designId, FactionId, position, Clan);
                    Modules.Add(module);
                }
            }
        }

        public override short GetAssetType() { return Clan.Id == 0 || buildTime.AddMinutes(BuildTimeInMinutes) > DateTime.Now ? ASSET_TYPE_NO_CLAN : ASSET_TYPE_CLAN_OWNING; }

        public override void Click(GameSession gameSession)
        {
            var player = gameSession.Player;
            UpdatePlayerModuleInventory(player);

            int secondsLeft = (int)(TimeSpan.FromMinutes(BuildTimeInMinutes).TotalSeconds - (DateTime.Now - buildTime).TotalSeconds);

            if (InBuildingState) player.SendCommand(BattleStationBuildingStateCommand.write(Id, Id, Name, secondsLeft, 0, Clan.Name, new FactionModule((short)FactionId)));
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
                                              new EquippedModulesModule(EquippedStationModule.ContainsKey(player.Clan.Id) ? EquippedStationModule[player.Clan.Id].Select(x => x.ModuleModule).ToList() : new List<StationModuleModule>()),
                                              (EquippedStationModule.ContainsKey(player.Clan.Id) ? EquippedStationModule[player.Clan.Id].Where(x => x.Installed).ToList().Count : 0) == 10),
                                      new AvailableModulesCommand(player.Storage.BattleStationModules),
                                      1,
                                      60,
                                      0));
                }
                else
                {
                    if (player.Clan.Id == Clan.Id)
                        player.SendCommand(BattleStationStatusCommand.write(Id, Id, Name, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new EquippedModulesModule(EquippedStationModule[player.Clan.Id].Select(x => x.ModuleModule).ToList()), false));
                }
            }
        }

        public override byte[] GetAssetCreateCommand(short clanRelationModule = ClanRelationModule.NONE)
        {
            return AssetCreateCommand.write(new AssetTypeModule(GetAssetType()), Name,
                                          FactionId, Clan.Tag, Id, DESIGN_ID, 0,
                                          Position.X, Position.Y, Clan.Id, true, true, true, true,
                                          new ClanRelationModule(clanRelationModule),
                                          Visuals);
        }
    }
}
