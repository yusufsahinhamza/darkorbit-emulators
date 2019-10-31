using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ow.Game.Objects;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net.netty.commands;
using Ow.Utils;

namespace Ow.Game.Objects.Players.Managers
{
    class DroneManager : AbstractManager
    {
        public List<int> Config1Designs = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public List<int> Config2Designs = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public bool Apis = false;
        public bool Zeus = false;

        public DroneManager(Player player) : base(player) { SetDroneDesigns(); }

        private const int DRONE_CHANGE_COOLDOWN_TIME = 3000;

        public const string DEFAULT_FORMATION = "drone_formation_default";
        public const string TURTLE_FORMATION = "drone_formation_f-01-tu";
        public const string ARROW_FORMATION = "drone_formation_f-02-ar";
        public const string LANCE_FORMATION = "drone_formation_f-03-la";
        public const string STAR_FORMATION = "drone_formation_f-04-st";
        public const string PINCER_FORMATION = "drone_formation_f-05-pi";
        public const string DOUBLE_ARROW_FORMATION = "drone_formation_f-06-da";
        public const string DIAMOND_FORMATION = "drone_formation_f-07-di";
        public const string CHEVRON_FORMATION = "drone_formation_f-08-ch";
        public const string MOTH_FORMATION = "drone_formation_f-09-mo";
        public const string CRAB_FORMATION = "drone_formation_f-10-cr";
        public const string HEART_FORMATION = "drone_formation_f-11-he";
        public const string BARRAGE_FORMATION = "drone_formation_f-12-ba";
        public const string BAT_FORMATION = "drone_formation_f-13-bt";
        public const string DOME_FORMATION = "drone_formation_f-3d-dm";
        public const string DRILL_FORMATION = "drone_formation_f-3d-dr";
        public const string RING_FORMATION = "drone_formation_f-3d-rg";
        public const string VETERAN_FORMATION = "drone_formation_f-3d-vt";
        public const string WHEEL_FORMATION = "drone_formation_f-3d-wl";
        public const string WAVE_FORMATION = "drone_formation_f-3d-wv";
        public const string X_FORMATION = "drone_formation_f-3d-x";

        public void Tick()
        {
            ShieldRegeneration();
            ShieldWeaken();
        }

        public void SetDroneDesigns()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var querySet = mySqlClient.ExecuteQueryRow($"SELECT * FROM player_equipment WHERE userId = {Player.Id}");
                dynamic config1Drones = JsonConvert.DeserializeObject(querySet["config1_drones"].ToString());
                dynamic config2Drones = JsonConvert.DeserializeObject(querySet["config2_drones"].ToString());
                dynamic items = JsonConvert.DeserializeObject(querySet["items"].ToString());

                Apis = items["apis"];
                Zeus = items["zeus"];

                for (var i = 0; i < 10; i++)
                {
                    foreach (var designId in config1Drones[i]["designs"])
                        Config1Designs[i] = (int)designId;

                    foreach (var designId in config2Drones[i]["designs"])
                        Config2Designs[i] = (int)designId;
                }
            }
        }

        public void UpdateDrones(bool updateItems = false)
        {
            if (updateItems)
            {
                Config1Designs = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                Config2Designs = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                SetDroneDesigns();
            }

            string drones = GetDronesPacket();
            Player.SendPacket(drones);
            Player.SendPacketToInRangePlayers(drones);

            var droneFormationChangeCommand = DroneFormationChangeCommand.write(Player.Id, GetSelectedFormationId(Player.Settings.InGameSettings.selectedFormation));
            Player.SendCommand(droneFormationChangeCommand);
            Player.SendCommandToInRangePlayers(droneFormationChangeCommand);
        }

        public string GetDronesPacket()
        {
            var DronePacket = "";

            if (Player.CurrentConfig == 1)
                DronePacket = $"2|6|{GetDesignId(Config1Designs[0])}|2|6|{GetDesignId(Config1Designs[1])}|2|6|{GetDesignId(Config1Designs[2])}|2|6|{GetDesignId(Config1Designs[3])}|2|6|{GetDesignId(Config1Designs[4])}|2|6|{GetDesignId(Config1Designs[5])}|2|6|{GetDesignId(Config1Designs[6])}|2|6|{GetDesignId(Config1Designs[7])}";
            else
                DronePacket = $"2|6|{GetDesignId(Config2Designs[0])}|2|6|{GetDesignId(Config2Designs[1])}|2|6|{GetDesignId(Config2Designs[2])}|2|6|{GetDesignId(Config2Designs[3])}|2|6|{GetDesignId(Config2Designs[4])}|2|6|{GetDesignId(Config2Designs[5])}|2|6|{GetDesignId(Config2Designs[6])}|2|6|{GetDesignId(Config2Designs[7])}";

            if (Apis)
                DronePacket += $"|3|6|{GetDesignId(Player.CurrentConfig == 1 ? Config1Designs[8] : Config2Designs[8])}";

            if (Zeus)
                DronePacket += $"|4|6|{GetDesignId(Player.CurrentConfig == 1 ? Config1Designs[9] : Config2Designs[9])}";

            var drones = "0|n|d|" + Player.Id + "|" + DronePacket;
            return drones;
        }

        public int GetDesignId(int designItemId)
        {
            if (designItemId >= 120 && designItemId < 130)
                return 1;
            else if (designItemId >= 130 && designItemId < 140)
                return 2;
            return 0;
        }

        public DateTime regenerationCooldown = new DateTime();
        public void ShieldRegeneration()
        {
            if (regenerationCooldown.AddSeconds(1) >= DateTime.Now || Player.Settings.InGameSettings.selectedFormation != DIAMOND_FORMATION || Player.CurrentShieldPoints >= Player.MaxShieldPoints) return;

            int regeneration = Maths.GetPercentage(Player.MaxShieldPoints, (Player.Settings.InGameSettings.selectedFormation == DIAMOND_FORMATION ? 1 : 0));

            Player.CurrentShieldPoints += (regeneration > 5000 ? 5000 : regeneration);
            Player.UpdateStatus();

            regenerationCooldown = DateTime.Now;
        }

        public DateTime shieldWeakenCooldown = new DateTime();
        public void ShieldWeaken()
        {
            if (shieldWeakenCooldown.AddSeconds(1) >= DateTime.Now || (Player.Settings.InGameSettings.selectedFormation != MOTH_FORMATION && Player.Settings.InGameSettings.selectedFormation != WHEEL_FORMATION)  || Player.CurrentShieldPoints <= 0) return;

            int amount = Maths.GetPercentage(Player.MaxShieldPoints, (Player.Settings.InGameSettings.selectedFormation == MOTH_FORMATION || Player.Settings.InGameSettings.selectedFormation == WHEEL_FORMATION ? 5 : 0));

            Player.CurrentShieldPoints -= amount;
            Player.UpdateStatus();

            shieldWeakenCooldown = DateTime.Now;
        }

        public DateTime formationCooldown = new DateTime();
        public void ChangeDroneFormation(string NewFormationID)
        {
            if (NewFormationID == Player.Settings.InGameSettings.selectedFormation) return;

            if (formationCooldown.AddMilliseconds(TimeManager.FORMATION_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.SendCooldown(DEFAULT_FORMATION, DRONE_CHANGE_COOLDOWN_TIME);

                string oldSelectedItem = Player.Settings.InGameSettings.selectedFormation;
                Player.Settings.InGameSettings.selectedFormation = NewFormationID;
                Player.SettingsManager.SendNewItemStatus(oldSelectedItem);
                Player.SettingsManager.SendNewItemStatus(NewFormationID);
                Player.Settings.InGameSettings.selectedFormation = NewFormationID;

                var formationCommand = DroneFormationChangeCommand.write(Player.Id, GetSelectedFormationId(NewFormationID));
                Player.SendCommand(formationCommand);
                Player.SendCommandToInRangePlayers(formationCommand);

                Player.UpdateStatus();
                Player.SettingsManager.SendNewItemStatus(NewFormationID);

                formationCooldown = DateTime.Now;
            }
        }

        public static int GetSelectedFormationId(string formation)
        {
            switch (formation)
            {
                case DEFAULT_FORMATION:
                    return 0;
                case TURTLE_FORMATION:
                    return 1;
                case ARROW_FORMATION:
                    return 2;
                case LANCE_FORMATION:
                    return 3;
                case STAR_FORMATION:
                    return 4;
                case PINCER_FORMATION:
                    return 5;
                case DOUBLE_ARROW_FORMATION:
                    return 6;
                case DIAMOND_FORMATION:
                    return 7;
                case CHEVRON_FORMATION:
                    return 8;
                case MOTH_FORMATION:
                    return 9;
                case CRAB_FORMATION:
                    return 10;
                case HEART_FORMATION:
                    return 11;
                case BARRAGE_FORMATION:
                    return 12;
                case BAT_FORMATION:
                    return 13;
                case RING_FORMATION:
                    return 14;
                case DRILL_FORMATION:
                    return 15;
                case VETERAN_FORMATION:
                    return 16;
                case DOME_FORMATION:
                    return 17;
                case WHEEL_FORMATION:
                    return 18;
                case X_FORMATION:
                    return 19;
                case WAVE_FORMATION:
                    return 20;
                default:
                    return 0;
            }
        }
    }
}
