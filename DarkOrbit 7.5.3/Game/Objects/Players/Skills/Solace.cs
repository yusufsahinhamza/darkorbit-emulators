using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty;
using Ow.Net.netty.commands;
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
            if (Player.Ship.Id == 63 && (cooldown.AddMilliseconds(TimeManager.SOLACE_COOLDOWN) < DateTime.Now || Player.Storage.GodMode))
            {
                ExecuteHeal();

                string packet = "0|SD|A|R|1|" + Player.Id + "";
                Player.SendPacket(packet);
                Player.SendPacketToInRangePlayers(packet);
                Player.SendCommand(AbilityStopCommand.write(1, Player.Id, new List<int>()));

                Player.SendCooldown(ServerCommands.SKILL_SOLACE, TimeManager.SOLACE_COOLDOWN);
                cooldown = DateTime.Now;
            }
        }

        public void ExecuteHeal()
        {
            int heal = Maths.GetPercentage(Player.MaxHitPoints, 50);
            if (Player.Group != null)
            {
                foreach(var player in Player.Group.Members.Values)
                {
                    if (player.Spacemap != Player.Spacemap) return;
                    player.Heal(heal);
                    if (player == Player) return;

                    string packet = "0|SD|A|R|1|" + player.Id + "";
                    player.SendPacket(packet);
                    player.SendPacketToInRangePlayers(packet);
                    player.SendCommand(AbilityStopCommand.write(1, player.Id, new List<int>()));
                }
            }
            else Player.Heal(heal);
        }
    }
}
