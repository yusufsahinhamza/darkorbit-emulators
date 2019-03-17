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
        public override int Id { get => 3; }

        public override int Duration { get => TimeManager.SPECTRUM_DURATION; }
        public override int Cooldown { get => TimeManager.SPECTRUM_COOLDOWN; }

        public Spectrum(Player player) : base(player) { }

        public override void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(TimeManager.SPECTRUM_DURATION) < DateTime.Now)
                    Disable();
            }
        }

        public override void Send()
        {
            if (Player.Ship.Id == 65 && cooldown.AddMilliseconds(TimeManager.SPECTRUM_DURATION + TimeManager.SPECTRUM_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.SkillManager.DisableAllSkills();

                Player.Storage.Spectrum = true;

                Player.AddVisualModifier(new VisualModifierCommand(Player.Id, VisualModifierCommand.PRISMATIC_SHIELD, 0, "", 0, true));

                Player.SendCooldown(SkillManager.SPECTRUM, TimeManager.SPECTRUM_DURATION, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public override void Disable()
        {
            Player.Storage.Spectrum = false;

            Player.RemoveVisualModifier(VisualModifierCommand.PRISMATIC_SHIELD);

            Player.SendCooldown(SkillManager.SPECTRUM, TimeManager.SPECTRUM_COOLDOWN);
            Active = false;
        }
    }
}
