using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;
using Ow.Game.Movements;
using Ow.Game.Clans;

namespace Ow.Game.Objects.Stations
{
    class HangarStation : Activatable
    {
        private static String ASSET_NAME = "Hangar";
        private static short ASSET_TYPE = AssetTypeModule.HANGAR_HOME;
        private static short DESIGN_ID = 0;

        public HangarStation(Spacemap spacemap, int factionId, Position position, Clan clan) : base(spacemap, factionId, position, clan) { }

        public override short GetAssetType() { return ASSET_TYPE; }

        public override void Click(GameSession gameSession)
        {

        }

        public override byte[] GetAssetCreateCommand()
        {
            return AssetCreateCommand.write(new AssetTypeModule(ASSET_TYPE), ASSET_NAME,
                                          FactionId, "", Id, DESIGN_ID, 0,
                                          Position.X, Position.Y, 0, true, true, true, false,
                                          new ClanRelationModule(ClanRelationModule.NONE),
                                          new List<VisualModifierCommand>());
        }
    }
}
