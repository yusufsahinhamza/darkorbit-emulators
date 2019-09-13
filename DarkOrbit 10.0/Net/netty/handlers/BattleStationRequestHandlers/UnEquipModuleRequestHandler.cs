using Ow.Game;
using Ow.Game.Objects.Stations;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests.BattleStationRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers.BattleStationRequestHandlers
{
    class UnEquipModuleRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new UnEquipModuleRequest();
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

                var module = battleStation.EquippedStationModule[player.Clan.Id].Where(x => x.ItemId == read.itemId).FirstOrDefault();

                if (module != null)
                {
                    if (module.OwnerId != player.Id)
                    {
                        player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.ITEM_NOT_OWNED));
                        return;
                    }

                    module.Remove();

                    battleStation.Click(gameSession);
                }
            }
        }
    }
}
