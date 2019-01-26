using Ow.Game.Objects.Players.Techs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Managers
{
    class TechManager
    {
        public Player Player { get; set; }

        public PrecisionTargeter PrecisionTargeter { get; set; }
        public BackupShields BackupShields { get; set; }
        public BattleRepairBot BattleRepairBot { get; set; }
        public EnergyLeech EnergyLeech { get; set; }
        public ChainImpulse ChainImpulse { get; set; }

        public TechManager(Player player) { Player = player; InitiateTechs(); }

        public void InitiateTechs()
        {
            PrecisionTargeter = new PrecisionTargeter(Player);
            BackupShields = new BackupShields(Player);
            BattleRepairBot = new BattleRepairBot(Player);
            EnergyLeech = new EnergyLeech(Player);
            ChainImpulse = new ChainImpulse(Player);
        }

        public void Tick()
        {
            PrecisionTargeter.Tick();
            BattleRepairBot.Tick();
            EnergyLeech.Tick();
        }

        public void AssembleTechCategoryRequest(string itemId)
        {
            switch (itemId)
            {
                case "1":
                    EnergyLeech.Send();
                    break;
                case "2":
                    ChainImpulse.Send();
                    break;
                case "3":
                    PrecisionTargeter.Send();
                    break;
                case "4":
                    BackupShields.Send();
                    break;
                case "5":
                    BattleRepairBot.Send();
                    break;
            }
            SendTechStatus();
        }

        public void SendTechStatus()
        {
            int repairRobotStatus = Player.TechManager.BattleRepairBot.Active ? 2 : 1;
            int energyLeechStatus = Player.TechManager.EnergyLeech.Active ? 2 : 1;
            int precisionTargeterStatus = Player.TechManager.PrecisionTargeter.Active ? 2 : 1;

            int repairRobotSeconds = repairRobotStatus == 2 ? (int)((TimeManager.BATTLE_REPAIR_BOT_DURATION / 1000) - (DateTime.Now - BattleRepairBot.cooldown).TotalSeconds) : (int)((TimeManager.BATTLE_REPAIR_BOT_DURATION + TimeManager.BATTLE_REPAIR_BOT_COOLDOWN/ 1000) - (DateTime.Now - BattleRepairBot.cooldown).TotalSeconds);
            int energyLeechSeconds = energyLeechStatus == 2 ? (int)((TimeManager.ENERGY_LEECH_DURATION / 1000) - (DateTime.Now - EnergyLeech.cooldown).TotalSeconds) : (int)((TimeManager.ENERGY_LEECH_DURATION + TimeManager.ENERGY_LEECH_COOLDOWN / 1000) - (DateTime.Now - EnergyLeech.cooldown).TotalSeconds);
            int precisionTargeterSeconds = precisionTargeterStatus == 2 ? (int)((TimeManager.PRECISION_TARGETER_DURATION / 1000) - (DateTime.Now - PrecisionTargeter.cooldown).TotalSeconds) : (int)((TimeManager.PRECISION_TARGETER_DURATION + TimeManager.PRECISION_TARGETER_COOLDOWN / 1000) - (DateTime.Now - PrecisionTargeter.cooldown).TotalSeconds);

            Player.SendPacket("0|TX|S|" + energyLeechStatus + "|99|"+ energyLeechSeconds + "|1|99|0|" + precisionTargeterStatus + "|99|" + precisionTargeterSeconds + "|1|99|0|" + repairRobotStatus + "|99|" + repairRobotSeconds);
        }

    }
}
