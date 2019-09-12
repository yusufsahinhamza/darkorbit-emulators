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

namespace Ow.Net.netty.handlers
{
    class EmergencyRepairRequestHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            var read = new EmergencyRepairRequest();
            read.readCommand(bytes);

            var player = gameSession.Player;
            var battleStation = player.Spacemap.GetActivatableMapEntity(read.battleStationId) as BattleStation;

            //TODO: REPAIR EFFECT

            if (battleStation != null)
            {
                var module = battleStation.EquippedStationModule[player.Clan.Id].Where(x => x.Module.slotId == read.slotId).FirstOrDefault();

                if (module != null)
                {
                    module.Destroyed = false;
                    module.Module.currentHitpoints = module.Module.maxHitpoints;
                    module.Module.currentShield = module.Module.maxShield;
                    module.DesignId = module.Module.type == StationModuleModule.REPAIR ? 3 : module.Module.type == StationModuleModule.LASER_HIGH_RANGE ? 4 : module.Module.type == StationModuleModule.LASER_MID_RANGE ? 5 : module.Module.type == StationModuleModule.LASER_LOW_RANGE ? 6 : module.Module.type == StationModuleModule.ROCKET_LOW_ACCURACY ? 7 : module.Module.type == StationModuleModule.ROCKET_MID_ACCURACY ? 8 : module.Module.type == StationModuleModule.HONOR_BOOSTER ? 9 : module.Module.type == StationModuleModule.DAMAGE_BOOSTER ? 10 : module.Module.type == StationModuleModule.EXPERIENCE_BOOSTER ? 11 : 0;

                    foreach (var character in module.Spacemap.Characters.Values)
                    {
                        if (character is Player)
                        {
                            short relationType = character.Clan.Id != 0 && module.Clan.Id != 0 ? module.Clan.GetRelation(character.Clan) : (short)0;
                            (character as Player).SendCommand(module.GetAssetCreateCommand(relationType));
                        }
                    }
                }
            }
        }
    }
}
