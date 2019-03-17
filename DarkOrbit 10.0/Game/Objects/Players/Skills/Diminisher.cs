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
        public override int Id { get => 2; }

        public override int Duration { get => TimeManager.DIMINISHER_DURATION; }
        public override int Cooldown { get => TimeManager.DIMINISHER_COOLDOWN; }

        public Diminisher(Player player) : base(player) { }

        public override void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(TimeManager.DIMINISHER_DURATION) < DateTime.Now)
                    Disable();
                if (Player.Selected == null || Player.Selected != Player.Storage.UnderDiminisherPlayer)
                    Disable();
            }
        }

        public override void Send()
        {
            if (Player.Ship.Id == 64 && cooldown.AddMilliseconds(TimeManager.DIMINISHER_DURATION + TimeManager.DIMINISHER_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                var enemy = Player.Selected;
                if (enemy == null || !(enemy is Player)) return;
                if (!Player.AttackManager.TargetDefinition(enemy as Player, false)) return;

                Player.SkillManager.DisableAllSkills();

                Player.Storage.Diminisher = true;
                Player.Storage.UnderDiminisherPlayer = enemy as Player;

                Player.AddVisualModifier(new VisualModifierCommand(Player.Id, VisualModifierCommand.WEAKEN_SHIELDS, 0, "", 0, true));
                (enemy as Player).AddVisualModifier(new VisualModifierCommand(enemy.Id, VisualModifierCommand.WEAKEN_SHIELDS, 0, "", 0, true));

                Player.SendCooldown(SkillManager.DIMINISHER, TimeManager.DIMINISHER_DURATION, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public override void Disable()
        {
            var enemy = Player.Storage.UnderDiminisherPlayer;
            if (enemy == null) return;

            Player.Storage.Diminisher = false;
            Player.Storage.UnderDiminisherPlayer = null;

            Player.RemoveVisualModifier(VisualModifierCommand.WEAKEN_SHIELDS);
            (enemy as Player).RemoveVisualModifier(VisualModifierCommand.WEAKEN_SHIELDS);

            Player.SendCooldown(SkillManager.DIMINISHER, TimeManager.DIMINISHER_COOLDOWN);
            Active = false;
        }
    }
}
