using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Diminisher
    {
        public Player Player { get; set; }

        public bool Active = false;

        public Diminisher(Player player)
        {
            Player = player;
        }

        public void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(TimeManager.DIMINISHER_DURATION) < DateTime.Now)
                    Disable();
                if (Player.Selected == null || Player.Selected != Player.UnderDiminisherPlayer)
                    Disable();
            }
        }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (cooldown.AddMilliseconds(TimeManager.DIMINISHER_DURATION + TimeManager.DIMINISHER_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                var enemy = Player.Selected;
                if (enemy == null || !(enemy is Player)) return;
                if (!Player.AttackManager.TargetDefinition(enemy as Player, false)) return;

                Player.Diminisher = true;
                Player.UnderDiminisherPlayer = enemy as Player;

                string packet = "0|SD|A|R|2|" + Player.Id + "";
                Player.SendPacket(packet);
                Player.SendPacketToInRangePlayers(packet);

                string packetEnemy = "0|SD|A|R|2|" + enemy.Id + "";
                Player.SendPacket(packetEnemy);
                Player.SendPacketToInRangePlayers(packetEnemy);

                Player.SendCooldown(SkillManager.DIMINISHER, TimeManager.DIMINISHER_DURATION, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public void Disable()
        {
            var enemy = Player.UnderDiminisherPlayer;
            if (enemy == null) return;

            Player.Diminisher = false;
            Player.UnderDiminisherPlayer = null;

            string packet = "0|SD|D|R|2|" + Player.Id + "";
            Player.SendPacket(packet);
            Player.SendPacketToInRangePlayers(packet);

            string packetEnemy = "0|SD|D|R|2|" + enemy.Id + "";
            Player.SendPacket(packetEnemy);
            Player.SendPacketToInRangePlayers(packetEnemy);

            Player.SendCooldown(SkillManager.DIMINISHER, TimeManager.DIMINISHER_COOLDOWN);
            Active = false;
        }
    }
}
