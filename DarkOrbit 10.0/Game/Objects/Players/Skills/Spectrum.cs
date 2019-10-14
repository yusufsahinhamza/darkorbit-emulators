using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Spectrum : Skill
    {
        public override string LootId { get => SkillManager.SPECTRUM; }

        public override int Duration { get => TimeManager.SPECTRUM_DURATION; }
        public override int Cooldown { get => TimeManager.SPECTRUM_COOLDOWN; }

        public Spectrum(Player player) : base(player) { }

        public override void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(Duration) < DateTime.Now)
                    Disable();
            }
        }

        public override void Send()
        {
            if (Ship.SPECTRUMS.Contains(Player.Ship.Id) && cooldown.AddMilliseconds(Duration + Cooldown) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.SkillManager.DisableAllSkills();

                Player.Storage.Spectrum = true;

                Player.AddVisualModifier(VisualModifierCommand.PRISMATIC_SHIELD, 0, "", 0, true);

                Player.SendCooldown(LootId, Duration, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public override void Disable()
        {
            Player.Storage.Spectrum = false;

            Player.RemoveVisualModifier(VisualModifierCommand.PRISMATIC_SHIELD);

            Player.SendCooldown(LootId, Cooldown);
            Active = false;
        }
    }
}
