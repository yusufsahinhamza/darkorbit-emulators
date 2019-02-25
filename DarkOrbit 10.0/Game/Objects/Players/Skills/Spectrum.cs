using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Spectrum
    {
        public Player Player { get; set; }

        public bool Active = false;

        public Spectrum(Player player) { Player = player; }

        public void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(TimeManager.SPECTRUM_DURATION) < DateTime.Now)
                    Disable();
            }
        }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (cooldown.AddMilliseconds(TimeManager.SPECTRUM_DURATION + TimeManager.SPECTRUM_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.SkillManager.DisableAllSkills();

                Player.Storage.Spectrum = true;

                Player.AddVisualModifier(new VisualModifierCommand(Player.Id, VisualModifierCommand.PRISMATIC_SHIELD, 0, "", 0, true));

                Player.SendCooldown(SkillManager.SPECTRUM, TimeManager.SPECTRUM_DURATION, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public void Disable()
        {
            Player.Storage.Spectrum = false;

            Player.RemoveVisualModifier(VisualModifierCommand.FORTRESS);

            Player.SendCooldown(SkillManager.SPECTRUM, TimeManager.SPECTRUM_COOLDOWN);
            Active = false;
        }
    }
}
