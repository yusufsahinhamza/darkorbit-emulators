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
                else if (Player.Selected == null || Player.Selected != Player.UnderVenomPlayer)
                    Disable();
                else
                    ExecuteDamage();
            }
        }

        public DateTime lastDamageTime = new DateTime();
        public void ExecuteDamage()
        {
            var enemy = Player.UnderVenomPlayer;
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
            if (cooldown.AddMilliseconds(TimeManager.VENOM_DURATION + TimeManager.VENOM_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                var enemy = Player.Selected;
                if (enemy == null || !(enemy is Player)) return;
                if (!Player.AttackManager.TargetDefinition(enemy as Player, false)) return;

                DAMAGE = 1500;
                Player.Venom = true;
                Player.UnderVenomPlayer = enemy as Player;

                string packet = "0|SD|A|R|5|" + Player.Id + "";
                Player.SendPacket(packet);
                Player.SendPacketToInRangePlayers(packet);

                string packetEnemy = "0|SD|A|R|5|" + enemy.Id + "";
                Player.SendPacket(packetEnemy);
                Player.SendPacketToInRangePlayers(packetEnemy);

                Player.SendCooldown(SkillManager.VENOM, TimeManager.VENOM_DURATION, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public void Disable()
        {
            var enemy = Player.UnderVenomPlayer;
            if (enemy == null) return;

            Player.Venom = false;
            Player.UnderVenomPlayer = null;

            string packet = "0|SD|D|R|5|" + Player.Id + "";
            Player.SendPacket(packet);
            Player.SendPacketToInRangePlayers(packet);

            string packetEnemy = "0|SD|D|R|5|" + enemy.Id + "";
            Player.SendPacket(packetEnemy);
            Player.SendPacketToInRangePlayers(packetEnemy);

            Player.SendCooldown(SkillManager.VENOM, TimeManager.VENOM_COOLDOWN);
            Active = false;
        }
    }
}
