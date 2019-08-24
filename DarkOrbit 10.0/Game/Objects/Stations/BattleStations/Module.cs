using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Stations.BattleStations
{
    class Module : Tick
    {
        public int TickId { get; set; }

        public BattleStation BattleStation { get; set; }
        public StationModuleModule ModuleModule { get; set; }
        public int OwnerId { get; set; }
        public bool Installed = false;

        public Module(BattleStation battleStation, StationModuleModule module, int ownerId)
        {
            BattleStation = battleStation;
            ModuleModule = module;
            OwnerId = ownerId;

            Program.TickManager.AddTick(this, out var tickId);
            TickId = tickId;
        }

        public DateTime installationTime = new DateTime();
        public void Tick()
        {
            if (!Installed)
            {
                if (ModuleModule.installationSecondsLeft > 0)
                {
                    var player = GameManager.GetPlayerById(OwnerId);

                    if (player != null && BattleStation != null && player.Position.DistanceTo(BattleStation.Position) < 700)
                    {
                        if (installationTime.AddSeconds(1) < DateTime.Now)
                        {
                            ModuleModule.installationSecondsLeft--;
                            installationTime = DateTime.Now;
                        }
                    } else Remove(true);
                }
                else if (ModuleModule.installationSecondsLeft <= 0)
                {
                    Installed = true;

                    var player = GameManager.GetPlayerById(OwnerId);

                    if (player != null && BattleStation != null)
                        BattleStation.Click(player.GameSession);
                }
            }
        }

        public void Remove(bool closeUI = false)
        {
            var player = GameManager.GetPlayerById(OwnerId);

            if (player != null && BattleStation != null)
            {
                BattleStation.EquippedStationModule[player.Clan.Id].Remove(this);

                if (BattleStation.EquippedStationModule[player.Clan.Id].Count == 0)
                    BattleStation.EquippedStationModule.Remove(player.Clan.Id);

                player.Storage.BattleStationModules.Add(ModuleModule);

                if (closeUI)
                    player.SendCommand(OutOfBattleStationRangeCommand.write(BattleStation.Id));
            }

            Program.TickManager.RemoveTick(this);
        }
    }
}
