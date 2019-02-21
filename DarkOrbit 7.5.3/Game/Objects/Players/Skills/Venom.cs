using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Venom
    {
        public Player Player { get; set; }

        public static int DAMAGE = 1500;
        public bool Active = false;

        public Venom(Player player)
        {
            Player = player;
        }

        public void Tick()
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

        public DateTime lastDamageTime = new DateTime();
        public void ExecuteDamage()
        {
            var enemy = Player.Storage.UnderVenomPlayer;
            if (enemy == null) return;
            if (!Player.AttackManager.TargetDefinition(enemy)) return;

            if (lastDamageTime.AddSeconds(1) < DateTime.Now)
            {
                AttackManager.Damage(Player, enemy, DamageType.SL, DAMAGE, true, true, false, false);
                DAMAGE += 200;

                lastDamageTime = DateTime.Now;
            }
        }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (Player.Ship.Id == 67 && (cooldown.AddMilliseconds(TimeManager.VENOM_DURATION + TimeManager.VENOM_COOLDOWN) < DateTime.Now || Player.Storage.GodMode))
            {
                var enemy = Player.Selected;
                if (enemy == null || !(enemy is Player)) return;
                if (!Player.AttackManager.TargetDefinition(enemy as Player, false)) return;

                DAMAGE = 1500;
                Player.Storage.Venom = true;
                Player.Storage.UnderVenomPlayer = enemy as Player;

                Player.AddVisualModifier(new VisualModifierCommand(Player.Id, VisualModifierCommand.SINGULARITY, 0, true));
                (enemy as Player).AddVisualModifier(new VisualModifierCommand(enemy.Id, VisualModifierCommand.SINGULARITY, 0, true));
                (enemy as Player).SendCommand(AbilityStopCommand.write(5, enemy.Id, new List<int>()));

                Player.SendCooldown(ServerCommands.SKILL_VENOM, TimeManager.VENOM_DURATION);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public void Disable()
        {
            var enemy = Player.Storage.UnderVenomPlayer;
            if (enemy == null) return;

            Player.Storage.Venom = false;
            Player.Storage.UnderVenomPlayer = null;

            Player.RemoveVisualModifier(VisualModifierCommand.SINGULARITY);
            (enemy as Player).RemoveVisualModifier(VisualModifierCommand.SINGULARITY);

            Player.SendCooldown(ServerCommands.SKILL_VENOM, TimeManager.VENOM_COOLDOWN);
            Active = false;
        }
    }
}
