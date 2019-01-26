using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Techs
{
    class EnergyLeech
    {
        public Player Player { get; set; }

        public bool Active = false;

        public EnergyLeech(Player player)
        {
            Player = player;
        }

        public void Tick()
        {
            if (Active)
                if (cooldown.AddMilliseconds(TimeManager.ENERGY_LEECH_DURATION) < DateTime.Now)
                    Disable();
        }

        public void ExecuteHeal(int damage)
        {
            int heal = Maths.GetPercentage(damage, 10);
            Player.Heal(heal);
        }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (cooldown.AddMilliseconds(TimeManager.ENERGY_LEECH_DURATION + TimeManager.ENERGY_LEECH_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                string packet = "0|TX|A|S|ELA|" + Player.Id;
                Player.SendPacket(packet);
                Player.SendPacketToInRangePlayers(packet);

                Player.EnergyLeech = true;
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public void Disable()
        {
            string packet = "0|TX|D|S|ELA|" + Player.Id;
            Player.SendPacket(packet);
            Player.SendPacketToInRangePlayers(packet);

            Player.EnergyLeech = false;
            Active = false;
            Player.TechManager.SendTechStatus();
            Player.SendCooldown(ServerCommands.TECH_ENERGY_LEECH, TimeManager.ENERGY_LEECH_COOLDOWN);
        }
    }
}
