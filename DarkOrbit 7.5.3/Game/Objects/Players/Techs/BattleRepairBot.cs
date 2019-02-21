using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Techs
{
    class BattleRepairBot
    {
        public Player Player { get; set; }
        private static int HEALT = 10000;
        public bool Active = false;

        public BattleRepairBot(Player player)
        {
            Player = player;
        }

        public void Tick()
        {
            if(Active)
                if (cooldown.AddMilliseconds(TimeManager.BATTLE_REPAIR_BOT_DURATION) < DateTime.Now)
                    Disable();
                else
                    ExecuteHeal();
        }

        public DateTime lastRepairTime = new DateTime();
        public void ExecuteHeal()
        {
            if (lastRepairTime.AddSeconds(1) < DateTime.Now)
            {
                int heal = HEALT;

                Player.Heal(heal);

                lastRepairTime = DateTime.Now;
            }
        }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (cooldown.AddMilliseconds(TimeManager.BATTLE_REPAIR_BOT_DURATION + TimeManager.BATTLE_REPAIR_BOT_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                string packet = "0|TX|A|S|BRB|" + Player.Id;
                Player.SendPacket(packet);
                Player.SendPacketToInRangePlayers(packet);

                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public void Disable()
        {
            string packet = "0|TX|D|S|BRB|" + Player.Id;
            Player.SendPacket(packet);
            Player.SendPacketToInRangePlayers(packet);

            Active = false;
            Player.TechManager.SendTechStatus();
            Player.SendCooldown(ServerCommands.TECH_BATTLE_REP_BOT, TimeManager.BATTLE_REPAIR_BOT_COOLDOWN);
        }
    }
}
