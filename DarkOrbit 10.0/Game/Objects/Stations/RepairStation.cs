using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ow.Utils;
using Ow.Game.Movements;
using Ow.Game;

namespace Ow.Game.Objects.Stations
{
    class RepairStation : Activatable
    {
        public RepairStation(Spacemap spacemap, int factionId, Position position, Clan clan) : base(spacemap, factionId, position, clan, AssetTypeModule.REPAIR_STATION) { }

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
            return AssetCreateCommand.write(GetAssetType(), "RepairDock",
                                          FactionId, "", Id, 0, 0,
                                          Position.X, Position.Y, 0, true, true, true, true,
                                          new ClanRelationModule(clanRelationModule),
                                          new List<VisualModifierCommand>());
        }
    }
}
