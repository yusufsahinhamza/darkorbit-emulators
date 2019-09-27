using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty.commands;
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

        public EnergyLeech(Player player) { Player = player; }

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
            if (cooldown.AddMilliseconds(TimeManager.ENERGY_LEECH_DURATION + TimeManager.ENERGY_LEECH_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.AddVisualModifier(VisualModifierCommand.ENERGY_LEECH_ARRAY, 0, "", 0, true);

                Player.SendCooldown(TechManager.TECH_ENERGY_LEECH, TimeManager.ENERGY_LEECH_DURATION, true);
                Player.Storage.EnergyLeech = true;
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public void Disable()
        {
            Player.RemoveVisualModifier(VisualModifierCommand.ENERGY_LEECH_ARRAY);

            Player.SendCooldown(TechManager.TECH_ENERGY_LEECH, TimeManager.ENERGY_LEECH_COOLDOWN);
            Player.Storage.EnergyLeech = false;
            Active = false;
        }
    }
}
