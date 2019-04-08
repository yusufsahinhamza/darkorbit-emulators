using Ow.Game;
using Ow.Game.Movements;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Stations
{
    class StationBase
    {
        public int TypeId { get; set; }
        public int FactionId { get; set; }
        public List<int> Position { get; set; }
    }

    class HomeStation : Activatable
    {
        private static String ASSET_NAME = "HQ";
        private static short ASSET_TYPE = AssetTypeModule.BASE_COMPANY;
        private static short DESIGN_ID = 0;
        public static int SECURE_ZONE_RANGE = 1500;

        public RepairStation RepairStation { get; set; }
        public HangarStation HangarStation { get; set; }
        public QuestGiverStation QuestGiverStation { get; set; }
        public OreTradeStation OreTradeStation { get; set; }

        public HomeStation(Spacemap spacemap, int factionId, Position position, Clan clan) : base(spacemap, factionId, position, clan)
        {
            PrepareStations();
        }

        public void PrepareStations()
        {
                var rPosition = new Position(Position.X + 0, Position.Y + -1080);
                var repairStation =
                        new RepairStation(Spacemap, FactionId, rPosition, Clan);
                RepairStation = repairStation;

                var hPosition = new Position(Position.X + +1080, Position.Y + 0);
                var hangarStation =
                        new HangarStation(Spacemap, FactionId, hPosition, Clan);
                HangarStation = hangarStation;

                var qPosition = new Position(Position.X + 0, Position.Y + 1080);
                var questGiverStation =
                        new QuestGiverStation(Spacemap, FactionId, qPosition, Clan);
                QuestGiverStation = questGiverStation;

                var oPosition = new Position(Position.X + -1080, Position.Y + -1);
                var oreTradeStation =
                        new OreTradeStation(Spacemap, FactionId, oPosition, Clan);
                OreTradeStation = oreTradeStation;
        }

        public override short GetAssetType() { return ASSET_TYPE; }

        public override void Click(GameSession gameSession)
        {

        }

        public override byte[] GetAssetCreateCommand(short clanRelationModule = ClanRelationModule.NONE)
        {
            return AssetCreateCommand.write(new AssetTypeModule(ASSET_TYPE), ASSET_NAME,
                                          FactionId, "", Id, DESIGN_ID, 0,
                                          Position.X, Position.Y, 0, true, true, true, false,
                                          new ClanRelationModule(clanRelationModule),
                                          new List<VisualModifierCommand>());
        }
    }
}
