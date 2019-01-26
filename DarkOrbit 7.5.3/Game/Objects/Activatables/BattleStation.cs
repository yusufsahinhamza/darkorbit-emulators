using Ow.Game.Clans;
using Ow.Game.Movements;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Activatables
{
    class BattleStation : Activatable
    {
        private static String ASSET_NAME = "Clan Battle Station";
        private static short ASSET_TYPE_CLAN_OWNING = AssetTypeModule.BATTLESTATION;
        private static short ASSET_TYPE_NO_CLAN = AssetTypeModule.ASTEROID;
        private static short DESIGN_ID = 2;

        public List<StationModuleModule> inventoryStationModule = new List<StationModuleModule>();
        public List<StationModuleModule> equippedStationModule = new List<StationModuleModule>();

        public BattleStation(Spacemap spacemap, int factionId, Position position, Clan clan) : base(spacemap, factionId, position, clan)
        {
            InitiateModules();
            PrepareSatellites();
        }

        public void InitiateModules()
        {
            equippedStationModule.Add(new StationModuleModule(Id, 2, 1, StationModuleModule.LASER_HIGH_RANGE, 50000, 50000, 50000, 50000, 1, "LEJYONER", 0, 0, 0, 0, 500));
            equippedStationModule.Add(new StationModuleModule(Id, 2, 2, StationModuleModule.LASER_HIGH_RANGE, 50000, 50000, 50000, 50000, 1, "LEJYONER", 0, 0, 0, 0, 500));
            equippedStationModule.Add(new StationModuleModule(Id, 2, 3, StationModuleModule.LASER_HIGH_RANGE, 50000, 50000, 50000, 50000, 1, "LEJYONER", 0, 0, 0, 0, 500));
            equippedStationModule.Add(new StationModuleModule(Id, 2, 4, StationModuleModule.LASER_HIGH_RANGE, 50000, 50000, 50000, 50000, 1, "LEJYONER", 0, 0, 0, 0, 500));
            equippedStationModule.Add(new StationModuleModule(Id, 2, 5, StationModuleModule.DAMAGE_BOOSTER, 50000, 50000, 50000, 50000, 1, "LEJYONER", 0, 0, 0, 0, 500));
            equippedStationModule.Add(new StationModuleModule(Id, 2, 6, StationModuleModule.DAMAGE_BOOSTER, 50000, 50000, 50000, 50000, 1, "LEJYONER", 0, 0, 0, 0, 500));
            equippedStationModule.Add(new StationModuleModule(Id, 2, 7, StationModuleModule.DAMAGE_BOOSTER, 50000, 50000, 50000, 50000, 1, "LEJYONER", 0, 0, 0, 0, 500));
            equippedStationModule.Add(new StationModuleModule(Id, 2, 8, StationModuleModule.DAMAGE_BOOSTER, 50000, 50000, 50000, 50000, 1, "LEJYONER", 0, 0, 0, 0, 500));
        }

        public void PrepareSatellites()
        {
            foreach (var satellite in equippedStationModule)
            {
                var center = Position;
                Position position = null;
                string name = "";

                switch (satellite.slotId)
                {
                    case 1:
                        position = new Position(center.X - 413, center.Y - 98);
                        name = "M-01";
                        break;
                    case 2:
                        position = new Position(center.X - 171, center.Y - 236);
                        name = "M-02";
                        break;
                    case 3:
                        position = new Position(center.X + 170, center.Y + 236);
                        name = "M-03";
                        break;
                    case 4:
                        position = new Position(center.X + 412, center.Y - 98);
                        name = "M-04";
                        break;
                    case 5:
                        position = new Position(center.X + 412, center.Y + 97);
                        name = "M-05";
                        break;
                    case 6:
                        position = new Position(center.X + 170, center.Y - 235);
                        name = "M-06";
                        break;
                    case 7:
                        position = new Position(center.X - 171, center.Y + 235);
                        name = "M-07";
                        break;
                    case 8:
                        position = new Position(center.X - 413, center.Y + 97);
                        name = "M-08";
                        break;
                }

                new Satellite(Spacemap, name, FactionId, position, Clan);
            }
        }

        public override short GetAssetType() { return Clan == null ? ASSET_TYPE_NO_CLAN : ASSET_TYPE_CLAN_OWNING; }

        public override void Click(GameSession gameSession)
        {
            var player = gameSession.Player;
            player.SendCommand(BattleStationBuildingUiInitializationCommand.write(Id, Id, "Test",
                                                                                      new AsteroidProgressCommand(
                                                                                              Id, 
                                                                                              (float)0.1,
                                                                                              (float)0.1, 
                                                                                              "En iyi", 
                                                                                              "Mevcut clan",
                                                                                              new EquippedModulesModule(equippedStationModule),
                                                                                              true),
                                                                                      new AvailableModulesCommand(inventoryStationModule),
                                                                                      1, 
                                                                                      1, 
                                                                                      1));
        }

        public override byte[] GetAssetCreateCommand()
        {
            return AssetCreateCommand.write(new AssetTypeModule(GetAssetType()), ASSET_NAME,
                                          FactionId, (Clan != null ? Clan.Tag : ""), Id, DESIGN_ID, 3,
                                          Position.X, Position.Y, (Clan != null ? Clan.Id : 0), true, true, true,
                                          new ClanRelationModule(ClanRelationModule.NONE),
                                          new List<VisualModifierCommand>());
        }
    }
}
