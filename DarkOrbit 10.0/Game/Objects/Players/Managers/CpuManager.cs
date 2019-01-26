using Ow.Game.Objects;
using Ow.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Managers
{
    class CpuManager
    {
        public const String CLK_XL = "equipment_extra_cpu_cl04k-xl";
        public const String AUTO_ROCKET_CPU = "equipment_extra_cpu_arol-x";
        public const String AUTO_HELLSTROM_CPU = "equipment_extra_cpu_rllb-x";
        public const String ROCKET_LAUNCHER = "equipment_weapon_rocketlauncher_hst-2";
        public const String GALAXY_JUMP_CPU = "equipment_extra_cpu_jp-02";

        private const int PREMIUM_CLOAK_COOLDOWN = 10000;
        private const int CLOAK_COOLDOWN = 20000;
        private const int CLOAK_PRICE = -256;

        public Player Player { get; set; }

        public CpuManager(Player player) { Player = player; }

        public DateTime CloakCooldown = new DateTime();
        public void Cloak()
        {
            if (CloakCooldown.AddMilliseconds(Player.Premium ? PREMIUM_CLOAK_COOLDOWN : CLOAK_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                CloakCooldown = DateTime.Now;
                if (Player.Spacemap.Id != 42 && Player.Spacemap.Id != 121)
                {
                    if (!Player.Invisible)
                    {
                        EnableCloak();
                        Player.SendCooldown(CLK_XL, Player.Premium ? PREMIUM_CLOAK_COOLDOWN : CLOAK_COOLDOWN);
                    }
                }
            }
        }

        public void ArolX()
        {
            if(!Player.AutoRocket)
                EnableArolX();
            else
                DisableArolX();
        }

        public void RllbX()
        {
            if (!Player.AutoRocketLauncher)
                EnableRllbX();
            else
                DisableRllbX();
        }

        public void EnableCloak()
        {
            Player.Invisible = true;
            string cloakPacket = "0|n|INV|" + Player.Id + "|1";
            Player.SendPacket(cloakPacket);
            Player.SendPacketToInRangePlayers(cloakPacket);

            var pet = Player.Pet;
            if (pet.Activated)
            {
                pet.Invisible = true;
                string petCloakPacket = "0|n|INV|" + pet.Id + "|1";
                pet.SendPacketToInRangePlayers(petCloakPacket);
            }

            Player.SettingsManager.SelectedCpus.Add(CLK_XL);

            if(!Player.Settings.InGameSettings.selectedCpus.Contains(CLK_XL))
                Player.Settings.InGameSettings.selectedCpus.Add(CLK_XL);

            Player.SettingsManager.SendNewItemStatus(CLK_XL);
            QueryManager.SavePlayer.Settings(Player);
        }

        public void DisableCloak()
        {
            Player.Invisible = false;
            string cloakPacket = "0|n|INV|" + Player.Id + "|0";
            Player.SendPacket(cloakPacket);
            Player.SendPacketToInRangePlayers(cloakPacket);
            Player.SettingsManager.SelectedCpus.Remove(CLK_XL);
            Player.Settings.InGameSettings.selectedCpus.Remove(CLK_XL);
            Player.SettingsManager.SendNewItemStatus(CLK_XL);
            QueryManager.SavePlayer.Settings(Player);
        }

        public void EnableArolX()
        {
            Player.AutoRocket = true;
            Player.SettingsManager.SelectedCpus.Add(AUTO_ROCKET_CPU);

            if (!Player.Settings.InGameSettings.selectedCpus.Contains(AUTO_ROCKET_CPU))
                Player.Settings.InGameSettings.selectedCpus.Add(AUTO_ROCKET_CPU);

            Player.SettingsManager.SendNewItemStatus(AUTO_ROCKET_CPU);
            QueryManager.SavePlayer.Settings(Player);
        }

        public void DisableArolX()
        {
            Player.AutoRocket = false;
            Player.SettingsManager.SelectedCpus.Remove(AUTO_ROCKET_CPU);
            Player.Settings.InGameSettings.selectedCpus.Remove(AUTO_ROCKET_CPU);
            Player.SettingsManager.SendNewItemStatus(AUTO_ROCKET_CPU);
            QueryManager.SavePlayer.Settings(Player);
        }

        public void EnableRllbX()
        {
            Player.AutoRocketLauncher = true;
            Player.SettingsManager.SelectedCpus.Add(AUTO_HELLSTROM_CPU);

            if (!Player.Settings.InGameSettings.selectedCpus.Contains(AUTO_HELLSTROM_CPU))
                Player.Settings.InGameSettings.selectedCpus.Add(AUTO_HELLSTROM_CPU);

            Player.SettingsManager.SendNewItemStatus(AUTO_HELLSTROM_CPU);
            QueryManager.SavePlayer.Settings(Player);
        }

        public void DisableRllbX()
        {
            Player.AutoRocketLauncher = false;
            Player.SettingsManager.SelectedCpus.Remove(AUTO_HELLSTROM_CPU);
            Player.Settings.InGameSettings.selectedCpus.Remove(AUTO_HELLSTROM_CPU);
            Player.SettingsManager.SendNewItemStatus(AUTO_HELLSTROM_CPU);
            QueryManager.SavePlayer.Settings(Player);
        }
    }
}
