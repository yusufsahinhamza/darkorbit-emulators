using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players
{
    class RocketLauncher
    {
        public Player Player { get; set; }

        public int CurrentLoad = 0;

        public int MaxLoad = 5;

        public int LoadRocketId { get; set; }

        public bool ReloadingActive = false;

        public RocketLauncher(Player player) { Player = player; LoadRocketId = AmmunitionTypeModule.HELLSTORM; }

        public void Tick()
        {
            if (ReloadingActive)
                Reload();
        }

        public DateTime LastReloadTime = new DateTime();
        public void Reload()
        {
            if (LastReloadTime.AddSeconds(Player.SettingsManager.SelectedFormation == DroneManager.STAR_FORMATION ? 0.67 : 1) > DateTime.Now) return;
            if (CurrentLoad == MaxLoad)
            {
                ReloadingActive = false;
                return;
            }

            ReloadingActive = true;
            CurrentLoad++;
            Player.SendPacket("0|"+ServerCommands.ROCKETLAUNCHER+"|"+ServerCommands.ROCKETLAUNCHER_STATUS+ "|2|" + Player.AttackManager.GetSelectedLauncherId() + "|" + CurrentLoad);
            LastReloadTime = DateTime.Now;
        }

        public void ChangeLoad(int rocketId)
        {
            ReloadingActive = false;
            CurrentLoad = 0;
            Player.SendPacket("0|" + ServerCommands.ROCKETLAUNCHER + "|" + ServerCommands.ROCKETLAUNCHER_STATUS + "|2|" + Player.AttackManager.GetSelectedLauncherId() + "|" + CurrentLoad);
            LoadRocketId = rocketId;
            LastReloadTime = DateTime.Now;
        }
    }
}
