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

        public Solace(Player player) { Player = player; }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (Player.Ship.Id == 63 && cooldown.AddMilliseconds(TimeManager.SOLACE_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.SkillManager.DisableAllSkills();

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
            int heal = Maths.GetPercentage(Player.MaxHitPoints, 25);
            if (Player.Group != null)
            {
                foreach (var player in Player.Group.Members.Values)
                {
                    if (player.Spacemap != Player.Spacemap) continue;
                    player.Heal(heal);
                    if (player == Player) continue;

                    string packet = "0|SD|A|R|1|" + player.Id + "";
                    player.SendPacket(packet);
                    player.SendPacketToInRangePlayers(packet);
                }
            }
            else Player.Heal(heal);
        }
    }
}
