using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Solace
    {
        public Player Player { get; set; }

        public Solace(Player player)
        {
            Player = player;
        }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (cooldown.AddMilliseconds(TimeManager.SOLACE_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                ExecuteHeal();

                string packet = "0|SD|A|R|1|" + Player.Id + "";
                Player.SendPacket(packet);
                Player.SendPacketToInRangePlayers(packet);

                Player.SendCooldown(SkillManager.SOLACE, TimeManager.SOLACE_COOLDOWN);
                cooldown = DateTime.Now;
            }
        }

        public void ExecuteHeal()
        {
            int damage = Maths.GetPercentage(Player.MaxHitPoints, 50);
            Player.Heal(damage);
        }
    }
}
