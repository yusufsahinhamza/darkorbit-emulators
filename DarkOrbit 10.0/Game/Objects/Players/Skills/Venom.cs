using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Venom : Skill
    {
        public override string LootId { get => SkillManager.VENOM; }
        public override int Id { get => 5; }

        public override int Duration { get => TimeManager.VENOM_DURATION; }
        public override int Cooldown { get => TimeManager.VENOM_COOLDOWN; }

        public int Damage = 1500;

        public Venom(Player player) : base(player) { }

        public override void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(TimeManager.VENOM_DURATION) < DateTime.Now)
                    Disable();
                else if (Player.Selected == null || Player.Selected != Player.Storage.UnderVenomPlayer)
                    Disable();
                else
                    ExecuteDamage();
            }
        }

        public override void Send()
        {
            if (Player.Ship.Id == 67 && cooldown.AddMilliseconds(TimeManager.VENOM_DURATION + TimeManager.VENOM_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                var enemy = Player.Selected;
                if (enemy == null || !(enemy is Player)) return;
                if (!Player.AttackManager.TargetDefinition(enemy as Player, false)) return;

                Player.SkillManager.DisableAllSkills();

                Damage = 1500;
                Player.Storage.Venom = true;
                Player.Storage.UnderVenomPlayer = enemy as Player;

                Player.AddVisualModifier(new VisualModifierCommand(Player.Id, VisualModifierCommand.SINGULARITY, 0, "", 0, true));
                (enemy as Player).AddVisualModifier(new VisualModifierCommand(enemy.Id, VisualModifierCommand.SINGULARITY, 0, "", 0, true));

                Player.SendCooldown(SkillManager.VENOM, TimeManager.VENOM_DURATION, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public override void Disable()
        {
            var enemy = Player.Storage.UnderVenomPlayer;
            if (enemy == null) return;

            Player.Storage.Venom = false;
            Player.Storage.UnderVenomPlayer = null;

            Player.RemoveVisualModifier(VisualModifierCommand.SINGULARITY);
            (enemy as Player).RemoveVisualModifier(VisualModifierCommand.SINGULARITY);

            Player.SendCooldown(SkillManager.VENOM, TimeManager.VENOM_COOLDOWN);
            Active = false;
        }

        public DateTime lastDamageTime = new DateTime();
        public void ExecuteDamage()
        {
            var enemy = Player.Storage.UnderVenomPlayer;
            if (enemy == null) return;
            if (!Player.AttackManager.TargetDefinition(enemy)) return;

            if (lastDamageTime.AddSeconds(1) < DateTime.Now)
            {
                AttackManager.Damage(Player, enemy, DamageType.SL, Damage, true, true, false, false);
                Damage += 200;

                lastDamageTime = DateTime.Now;
            }
        }
    }
}
