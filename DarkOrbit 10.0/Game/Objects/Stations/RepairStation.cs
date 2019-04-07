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
    class RepairStation : Activatable
    {
        private static String ASSET_NAME = "RepairDock";
        private static short ASSET_TYPE = AssetTypeModule.REPAIR_STATION;
        private static short DESIGN_ID = 0;

        public RepairStation(Spacemap spacemap, int factionId, Position position, Clan clan) : base(spacemap, factionId, position, clan) { }

        public override short GetAssetType() { return ASSET_TYPE; }

        public override void Click(GameSession gameSession)
        {
            var player = gameSession.Player;
            if (player.FactionId != FactionId) return;
            if (player.CurrentHitPoints == player.MaxHitPoints) return;
            if (player.AttackingOrUnderAttack()) return;

            int heal = player.MaxHitPoints;
            player.Heal(heal);
            player.SendPacket("0|A|STM|repsuccess");
        }

        public override byte[] GetAssetCreateCommand(short clanRelationModule = ClanRelationModule.NONE)
        {
            return AssetCreateCommand.write(new AssetTypeModule(ASSET_TYPE), ASSET_NAME,
                                          FactionId, "", Id, DESIGN_ID, 0,
                                          Position.X, Position.Y, 0, true, true, true, true,
                                          new ClanRelationModule(clanRelationModule),
                                          new List<VisualModifierCommand>());
        }
    }
}
