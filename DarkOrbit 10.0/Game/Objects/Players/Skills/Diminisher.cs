using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Diminisher : Skill
    {
        public override string LootId { get => SkillManager.DIMINISHER; }

        public override int Duration { get => TimeManager.DIMINISHER_DURATION; }
        public override int Cooldown { get => TimeManager.DIMINISHER_COOLDOWN; }

        public Diminisher(Player player) : base(player) { }

        public override void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(Duration) < DateTime.Now)
                    Disable();
                if (Player.Storage.UnderDiminisherEntity == null || Player.Selected != Player.Storage.UnderDiminisherEntity || Player.Spacemap.Id != Player.Storage.UnderDiminisherEntity.Spacemap.Id)
                    Disable();
            }
        }

        public override void Send()
        {
            if (Ship.DIMINISHERS.Contains(Player.Ship.Id) && cooldown.AddMilliseconds(Duration + Cooldown) < DateTime.Now || Player.Storage.GodMode)
            {
                var target = Player.Selected;
                if (target == null) return;
                if (!Player.TargetDefinition(target, false)) return;

                Player.SkillManager.DisableAllSkills();

                Player.Storage.Diminisher = true;
                Player.Storage.UnderDiminisherEntity = target;

                Player.AddVisualModifier(VisualModifierCommand.WEAKEN_SHIELDS, 0, "", 0, true);
                target.AddVisualModifier(VisualModifierCommand.WEAKEN_SHIELDS, 0, "", 0, true);

                Player.SendCooldown(LootId, Duration, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public override void Disable()
        {
            var target = Player.Storage.UnderDiminisherEntity;

            Player.Storage.Diminisher = false;
            Player.Storage.UnderDiminisherEntity = null;

            Player.RemoveVisualModifier(VisualModifierCommand.WEAKEN_SHIELDS);

            if (target != null)
                target.RemoveVisualModifier(VisualModifierCommand.WEAKEN_SHIELDS);

            Player.SendCooldown(LootId, Cooldown);
            Active = false;
        }
    }
}
