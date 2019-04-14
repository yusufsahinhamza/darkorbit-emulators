using Ow.Game;
using Ow.Game.Movements;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
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

        public List<StationModuleModule> inventoryStationModule = new List<StationModuleModule>();
        public List<StationModuleModule> equippedStationModule = new List<StationModuleModule>();
        public List<VisualModifierCommand> Visuals = new List<VisualModifierCommand>();
        public List<Satellite> Modules = new List<Satellite>();

        public int TickId { get; set; }
        public string Name => Clan.Id != 0 ? Clan.Name : "Julius";
        public bool InBuildingState = false;
        public int BuildTimeInMinutes = 0;

        //public DateTime ModuleInstallationSeconds = new DateTime();
        //public bool ModuleInstallationState => ModuleInstallationSeconds.AddSeconds(60) < DateTime.Now;

        public BattleStation(Spacemap spacemap, int factionId, Position position, Clan clan) : base(spacemap, factionId, position, clan)
        {
            InitiateStationInventory();
            
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

        public void InitiateStationInventory()
        {
            for (var i = 1; i <= 8; i++)
                inventoryStationModule.Add(new StationModuleModule(Id, i, i, StationModuleModule.LASER_HIGH_RANGE, 1, 1, 1, 1, 16, Clan.Tag, 60, 0 , 0, 0, 500));
            for (var i = 9; i <= 16; i++)
                inventoryStationModule.Add(new StationModuleModule(Id, i, i, StationModuleModule.ROCKET_LOW_ACCURACY, 1, 1, 1, 1, 16, Clan.Tag, 60, 0, 0, 0, 500));
            for (var i = 15; i <= 22; i++)
                inventoryStationModule.Add(new StationModuleModule(Id, i, i, StationModuleModule.ROCKET_MID_ACCURACY, 1, 1, 1, 1, 16, Clan.Tag, 60, 0, 0, 0, 500));

            inventoryStationModule.Add(new StationModuleModule(Id, 23, 23, StationModuleModule.DAMAGE_BOOSTER, 1, 1, 1, 1, 16, Clan.Tag, 60, 0, 0, 0, 500));
            inventoryStationModule.Add(new StationModuleModule(Id, 24, 24, StationModuleModule.EXPERIENCE_BOOSTER, 1, 1, 1, 1, 16, Clan.Tag, 60, 0, 0, 0, 500));
            inventoryStationModule.Add(new StationModuleModule(Id, 25, 25, StationModuleModule.HONOR_BOOSTER, 1, 1, 1, 1, 16, Clan.Tag, 60, 0, 0, 0, 500));
            inventoryStationModule.Add(new StationModuleModule(Id, 26, 26, StationModuleModule.REPAIR, 1, 1, 1, 1, 16, Clan.Tag, 60, 0, 0, 0, 500));
            inventoryStationModule.Add(new StationModuleModule(Id, DEFLECTOR_ID, DEFLECTOR_ID, StationModuleModule.DEFLECTOR, 1, 1, 1, 1, 16, Clan.Tag, 60, 0, 0, 0, 500));
            inventoryStationModule.Add(new StationModuleModule(Id, HULL_ID, HULL_ID, StationModuleModule.HULL, 1, 1, 1, 1, 16, Clan.Tag, 60, 0, 0, 0, 500));
        }

        public void InitiateModules()
        {
            //add to equippedStationModule, setted modules
        }

        public void PrepareSatellites()
        {
            foreach (var satellite in equippedStationModule)
            {
                if (satellite.itemId == DEFLECTOR_ID || satellite.itemId == HULL_ID) continue;

                var center = Position;
                int designId = satellite.type == StationModuleModule.REPAIR ? 3 : satellite.type == StationModuleModule.LASER_HIGH_RANGE ? 4 : satellite.type == StationModuleModule.LASER_MID_RANGE ? 5 : satellite.type == StationModuleModule.LASER_LOW_RANGE ? 6 : satellite.type == StationModuleModule.ROCKET_LOW_ACCURACY ? 7 : satellite.type == StationModuleModule.ROCKET_MID_ACCURACY ? 8 : satellite.type == StationModuleModule.HONOR_BOOSTER ? 9 : satellite.type == StationModuleModule.DAMAGE_BOOSTER ? 10 : satellite.type == StationModuleModule.EXPERIENCE_BOOSTER ? 11 : 0;
                string name = satellite.type == StationModuleModule.REPAIR ? "REPM-1" : satellite.type == StationModuleModule.LASER_HIGH_RANGE ? "LTM-HR" : satellite.type == StationModuleModule.LASER_MID_RANGE ? "LTM-MR" : satellite.type == StationModuleModule.LASER_LOW_RANGE ? "LTM-LR" : satellite.type == StationModuleModule.ROCKET_LOW_ACCURACY ? "RAM-LA" : satellite.type == StationModuleModule.ROCKET_MID_ACCURACY ? "RAM-MA" : satellite.type == StationModuleModule.HONOR_BOOSTER ? "HONM-1" : satellite.type == StationModuleModule.DAMAGE_BOOSTER ? "DMGM-1" : satellite.type == StationModuleModule.EXPERIENCE_BOOSTER ? "XPM-1" : "";
                var position = satellite.slotId == 9 ? new Position(center.X - 171, center.Y - 236) : satellite.slotId == 2 ? new Position(center.X + 170, center.Y - 235) : satellite.slotId == 3 ? new Position(center.X + 412, center.Y - 98) : satellite.slotId == 4 ? new Position(center.X + 412, center.Y + 97) : satellite.slotId == 5 ? new Position(center.X + 170, center.Y + 236) : satellite.slotId == 6 ? new Position(center.X - 171, center.Y + 235) : satellite.slotId == 7 ? new Position(center.X - 413, center.Y + 97) : satellite.slotId == 8 ? new Position(center.X - 413, center.Y - 98) : null;

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
            //TODO: zamanı buildTime ın kalan zamanına göre ayarla
            if (InBuildingState) player.SendCommand(BattleStationBuildingStateCommand.write(Id, Id, Name, BuildTimeInMinutes * 60, 3600, Clan.Name, new FactionModule((short)FactionId)));
            else
            {
                if (Clan.Id == 0 || buildTime.AddMinutes(BuildTimeInMinutes) > DateTime.Now)
                {
                    player.SendCommand(BattleStationBuildingUiInitializationCommand.write(Id, Id, Name,
                                      new AsteroidProgressCommand(
                                              Id,
                                              (float)equippedStationModule.Count / 10,
                                              (float)0.1,
                                              "En iyi",
                                              "Mevcut clan",
                                              new EquippedModulesModule(equippedStationModule),
                                              equippedStationModule.Count == 10),
                                      new AvailableModulesCommand(inventoryStationModule),
                                      1,
                                      60,
                                      0));
                }
                else player.SendCommand(BattleStationStatusCommand.write(Id, Id, Name, false, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new EquippedModulesModule(equippedStationModule), true));
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
