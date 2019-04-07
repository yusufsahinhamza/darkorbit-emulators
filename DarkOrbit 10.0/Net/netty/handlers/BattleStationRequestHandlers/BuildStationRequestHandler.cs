using Ow.Game;
using Ow.Game.Objects.Stations;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests.BattleStationRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers.BattleStationRequestHandlers
{
    class BuildStationRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new BuildStationRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var activatableStationaryMapEntity = player.Spacemap.GetActivatableMapEntity(read.battleStationId);

            if (activatableStationaryMapEntity != null && activatableStationaryMapEntity is BattleStation battleStation)
            {
                if (player.Position.DistanceTo(activatableStationaryMapEntity.Position) > 700)
                {
                    player.SendCommand(BattleStationErrorCommand.write(BattleStationErrorCommand.OUT_OF_RANGE));
                    return;
                }

                if (battleStation.equippedStationModule.Count != 10 || battleStation.InBuildingState) return;

                battleStation.Clan = player.Clan;
                battleStation.FactionId = battleStation.Clan.FactionId;
                battleStation.BuildTimeInMinutes = read.buildTimeInMinutes;
                battleStation.InBuildingState = true;

                var tickId = battleStation.TickId;
                Program.TickManager.AddTick(battleStation, out tickId);

                battleStation.buildTime = DateTime.Now;

                var visualCommand = new VisualModifierCommand(battleStation.Id, VisualModifierCommand.BATTLESTATION_CONSTRUCTING, 0, "", 0, true);
                battleStation.Visuals.Add(visualCommand);
                GameManager.SendCommandToMap(battleStation.Spacemap.Id, visualCommand.writeCommand());

                player.SendCommand(BattleStationBuildingStateCommand.write(battleStation.Id, battleStation.Id, battleStation.Name, battleStation.BuildTimeInMinutes * 60, 3600, battleStation.Clan.Name, new FactionModule((short)battleStation.FactionId)));
            }
        }
    }
}
