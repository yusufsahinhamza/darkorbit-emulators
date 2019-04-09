using Ow.Game.Objects.Players.Managers;
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

        public string LoadLootId { get; set; }

        public bool ReloadingActive = false;

        public RocketLauncher(Player player) { Player = player; LoadLootId = AmmunitionManager.ECO_10; }

        public void Tick()
        {
            if (ReloadingActive)
                Reload();
        }

        public DateTime LastReloadTime = new DateTime();
        public void Reload()
        {
            if (LastReloadTime.AddSeconds(Player.RocketLauncherSpeed) > DateTime.Now) return;
            if (CurrentLoad == MaxLoad)
            {
                ReloadingActive = false;
                return;
            }

            ReloadingActive = true;
            CurrentLoad++;
            Player.SendPacket("0|RL|S|2|" + Player.AttackManager.GetSelectedLauncherId() + "|" + CurrentLoad);
            Player.SettingsManager.SendNewItemStatus(CpuManager.ROCKET_LAUNCHER);
            LastReloadTime = DateTime.Now;
        }

        public void ChangeLoad(string lootId)
        {
            ReloadingActive = false;
            CurrentLoad = 0;
            LoadLootId = lootId;
            LastReloadTime = DateTime.Now;
            Player.SettingsManager.SendNewItemStatus(CpuManager.ROCKET_LAUNCHER);
        }
    }
}
