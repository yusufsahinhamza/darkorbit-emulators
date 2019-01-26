using Ow.Game.Clans;
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
    class Satellite : Activatable
    {
        private static String ASSET_NAME = "Satellite";
        private static short ASSET_TYPE = 56;
        private static short DESIGN_ID = 0;

        public Satellite(Spacemap pCurrentSpaceMapId, string name, int pFactionId, Position position, Clan clan) : base(pCurrentSpaceMapId, pFactionId, position, clan)
        {
            ASSET_NAME = name;
        }

        public override short GetAssetType() { return ASSET_TYPE; }

        public override void Click(GameSession gameSession)
        {

        }

        public override byte[] GetAssetCreateCommand()
        {
            return AssetCreateCommand.write(new AssetTypeModule(ASSET_TYPE), ASSET_NAME,
                                          FactionId, "", Id, DESIGN_ID, 0,
                                          Position.X, Position.Y, 0, false, true, true, true,
                                          new ClanRelationModule(ClanRelationModule.NONE),
                                          new List<VisualModifierCommand>());
        }
    }
}
