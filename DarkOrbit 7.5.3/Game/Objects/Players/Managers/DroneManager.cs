using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ow.Game.Objects;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net.netty;
using Ow.Net.netty.commands;
using Ow.Utils;

namespace Ow.Game.Objects.Players.Managers
{
    class DroneManager
    {
        public Player Player { get; set; }
        public List<int> Config1Designs = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public List<int> Config2Designs = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public bool Apis = false;
        public bool Zeus = false;

        public DroneManager(Player player) { Player = player; SetDroneDesigns(); }

        private const int DRONE_CHANGE_COOLDOWN_TIME = 3000;

        public const int DEFAULT_FORMATION = 0;
        public const int TURTLE_FORMATION = 1;
        public const int ARROW_FORMATION = 2;
        public const int LANCE_FORMATION = 3;
        public const int STAR_FORMATION = 4;
        public const int PINCER_FORMATION = 5;
        public const int DOUBLE_ARROW_FORMATION = 6;
        public const int DIAMOND_FORMATION = 7;
        public const int CHEVRON_FORMATION = 8;
        public const int MOTH_FORMATION = 9;
        public const int CRAB_FORMATION = 10;
        public const int HEART_FORMATION = 11;
        public const int BARRAGE_FORMATION = 12;
        public const int BAT_FORMATION = 13;
        public const int MOSQUITO_FORMATION = 14;
        public const int X_FORMATION = 42;

        public void Tick()
        {
            DiamondRegeneration();
            MothWeaken();
        }

        public void SetDroneDesigns()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                string sql = $"SELECT * FROM player_equipment WHERE userId = {Player.Id} ";
                var querySet = mySqlClient.ExecuteQueryRow(sql);
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

        public void UpdateDrones()
        {
            Config1Designs = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Config2Designs = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            SetDroneDesigns();

            string drones = GetDronesPacket();
            Player.SendPacket(drones);
            Player.SendPacketToInRangePlayers(drones);

            var droneFormationChangeCommand = DroneFormationChangeCommand.write(Player.Id, (int)Player.SettingsManager.SelectedFormation);
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
            {
                if (Player.CurrentConfig == 1)
                    DronePacket += $"|3|6|{GetDesignId(Config1Designs[8])}";
                else
                    DronePacket += $"|3|6|{GetDesignId(Config2Designs[8])}";
            }

            if (Zeus)
            {
                if (Player.CurrentConfig == 1)
                    DronePacket += $"|4|6|{GetDesignId(Config1Designs[9])}";
                else
                    DronePacket += $"|4|6|{GetDesignId(Config2Designs[9])}";
            }

            var drones = "0|n|d|" + Player.Id + "|" + DronePacket;
            return drones;
        }

        public int GetDesignId(int designItemId)
        {
            if (designItemId >= 85 && designItemId < 95)
                return 1;
            else if (designItemId >= 95 && designItemId < 105)
                return 2;
            return 0;
        }

        public DateTime regenerationCooldown = new DateTime();
        public void DiamondRegeneration()
        {
            if (regenerationCooldown.AddSeconds(1) >= DateTime.Now || Player.SettingsManager.SelectedFormation != DroneManager.DIAMOND_FORMATION || Player.CurrentShieldPoints >= Player.MaxShieldPoints) return;

            int regeneration = Maths.GetPercentage(Player.MaxShieldPoints, 1);

            Player.CurrentShieldPoints += regeneration > 5000 ? 5000 : regeneration;
            Player.UpdateStatus();

            regenerationCooldown = DateTime.Now;
        }

        public DateTime mothWeakenCooldown = new DateTime();
        public void MothWeaken()
        {
            if (mothWeakenCooldown.AddSeconds(1) >= DateTime.Now || Player.SettingsManager.SelectedFormation != DroneManager.MOTH_FORMATION || Player.CurrentShieldPoints <= 0) return;

            int amount = Maths.GetPercentage(Player.MaxShieldPoints, 1);

            Player.CurrentShieldPoints -= amount;
            Player.UpdateStatus();

            mothWeakenCooldown = DateTime.Now;
        }

        public DateTime formationCooldown = new DateTime();
        public void ChangeDroneFormation(int NewFormationID)
        {
            if (NewFormationID == (int)Player.SettingsManager.SelectedFormation) return;

            if (formationCooldown.AddMilliseconds(TimeManager.FORMATION_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                Player.SendCooldown(ServerCommands.DRONE_FORMATION_COOLDOWN, 3000);

                Player.SettingsManager.SelectedFormation = (short)NewFormationID;
                Player.Settings.InGameSettings.selectedFormation = NewFormationID;

                var formationCommand = DroneFormationChangeCommand.write(Player.Id, NewFormationID);
                Player.SendCommand(formationCommand);
                Player.SendCommandToInRangePlayers(formationCommand);

                Player.UpdateStatus();

                QueryManager.SavePlayer.Settings(Player);

                formationCooldown = DateTime.Now;
            }
        }
    }
}
