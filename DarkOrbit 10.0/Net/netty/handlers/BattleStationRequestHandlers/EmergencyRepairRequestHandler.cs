using Ow.Game;
using Ow.Game.Objects;
using Ow.Game.Objects.Stations;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers.BattleStationRequestHandlers
{
    class EmergencyRepairRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new EmergencyRepairRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var battleStation = player.Spacemap.GetActivatableMapEntity(read.battleStationId) as BattleStation;

            if (battleStation != null)
            {
                var module = battleStation.EquippedStationModule[player.Clan.Id].Where(x => x.SlotId == read.slotId).FirstOrDefault();

                if (module != null && battleStation.AssetTypeId == AssetTypeModule.BATTLESTATION && !module.EmergencyRepairActive)
                    Repair(1200, module);
            }
        }

        public async void Repair(int seconds, Satellite satellite)
        {
            //You can start multiple emergency repairs right after one another, but the first emergency repair need to finish before you can start the next.

            var activatable = satellite.Type == StationModuleModule.HULL || satellite.Type == StationModuleModule.DEFLECTOR ? (Activatable)satellite.BattleStation : satellite;

            if (activatable != null && activatable.CurrentHitPoints < activatable.MaxHitPoints)
            {
                satellite.EmergencyRepairActive = true;
                activatable.AddVisualModifier(VisualModifierCommand.EMERGENCY_REPAIR, 0, "", 0, true);

                for (int i = seconds; i > 0; i--)
                {
                    activatable.Heal(7500);
                    await Task.Delay(1000);

                    if (i <= 1 || activatable.CurrentHitPoints >= activatable.MaxHitPoints)
                    {
                        satellite.EmergencyRepairActive = false;
                        activatable.RemoveVisualModifier(VisualModifierCommand.EMERGENCY_REPAIR);
                        break;
                    }
                }   
            }
        }
    }
}
