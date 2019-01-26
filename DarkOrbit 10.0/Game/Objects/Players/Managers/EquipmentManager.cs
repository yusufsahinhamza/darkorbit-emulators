using Ow.Game.Objects;
using Ow.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Managers
{
    class EquipmentManager
    {
        public Player Player { get; set; }

        public int damageCfg1 = 10000;
        public int shieldCfg1 = 575000;

        public int damageCfg2 = 10000;
        public int shieldCfg2 = 575000;

        public EquipmentManager(Player player) { Player = player; /*LoadConfigs();*/ }

        public void LoadConfigs()
        {
            damageCfg1 = QueryManager.LoadItem(Player.Id, "equipment_weapon_laser_lf_3", 1, false);
            damageCfg1 += QueryManager.LoadItem(Player.Id, "equipment_drones_weapon_laser_lf_3", 1, true);

            shieldCfg1 = QueryManager.LoadItem(Player.Id, "equipment_weapon_shield_b0_2", 1, false);
            shieldCfg1 += QueryManager.LoadItem(Player.Id, "equipment_drones_weapon_shield_b0_2", 1, true);


            damageCfg2 = QueryManager.LoadItem(Player.Id, "equipment_weapon_laser_lf_3", 2, false);
            damageCfg2 += QueryManager.LoadItem(Player.Id, "equipment_drones_weapon_laser_lf_3", 2, true);

            shieldCfg2 = QueryManager.LoadItem(Player.Id, "equipment_weapon_shield_b0_2", 2, false);
            shieldCfg2 += QueryManager.LoadItem(Player.Id, "equipment_drones_weapon_shield_b0_2", 2, true);
        }

    }
}
