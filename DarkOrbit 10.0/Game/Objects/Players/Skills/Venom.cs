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

        public override int Duration { get => TimeManager.VENOM_DURATION; }
        public override int Cooldown { get => TimeManager.VENOM_COOLDOWN; }

        public int Damage = 1500;

        public Venom(Player player) : base(player) { }

        public override void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(Duration) < DateTime.Now)
                    Disable();
                else if (Player.Storage.UnderVenomEntity == null || Player.Selected != Player.Storage.UnderVenomEntity || Player.Spacemap.Id != Player.Storage.UnderVenomEntity.Spacemap.Id)
                    Disable();
                else
                    ExecuteDamage();
            }
        }

        public override void Send()
        {
            if (Player.Ship.Id == Ship.GOLIATH_VENOM && cooldown.AddMilliseconds(Duration + Cooldown) < DateTime.Now || Player.Storage.GodMode)
            {
                var target = Player.Selected;
                if (target == null) return;
                if (!Player.TargetDefinition(target, false)) return;

                Player.SkillManager.DisableAllSkills();

                Damage = 1500;
                Player.Storage.Venom = true;
                Player.Storage.UnderVenomEntity = target;

                Player.AddVisualModifier(VisualModifierCommand.SINGULARITY, 0, "", 0, true);
                target.AddVisualModifier(VisualModifierCommand.SINGULARITY, 0, "", 0, true);

                Player.SendCooldown(LootId, Duration, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public override void Disable()
        {
            var target = Player.Storage.UnderVenomEntity;

            Player.Storage.Venom = false;
            Player.Storage.UnderVenomEntity = null;

            Player.RemoveVisualModifier(VisualModifierCommand.SINGULARITY);

            if (target != null)
                target.RemoveVisualModifier(VisualModifierCommand.SINGULARITY);

            Player.SendCooldown(LootId, Cooldown);
            Active = false;
        }

        public DateTime lastDamageTime = new DateTime();
        public void ExecuteDamage()
        {
            var target = Player.Storage.UnderVenomEntity;
            if (target == null) return;           

            if (lastDamageTime.AddSeconds(1) < DateTime.Now)
            {
                AttackManager.Damage(Player, target, DamageType.SL, Damage, true, true, false, false);
                Damage += 200;

                lastDamageTime = DateTime.Now;
            }
        }
    }
}
