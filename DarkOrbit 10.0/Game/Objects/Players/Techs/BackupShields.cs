using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Techs
{
    class BackupShields
    {
        public Player Player { get; set; }

        public static int SHIELD = 75000;

        public BackupShields(Player player) { Player = player; }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (cooldown.AddMilliseconds(TimeManager.BACKUP_SHIELD_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                string packet = "0|TX|A|S|SBU|" + Player.Id;
                Player.SendPacket(packet);
                Player.SendPacketToInRangePlayers(packet);

                Player.Heal(SHIELD, Player.Id, HealType.SHIELD);

                Player.SendCooldown(TechManager.TECH_BACKUP_SHIELDS, TimeManager.BACKUP_SHIELD_COOLDOWN);

                cooldown = DateTime.Now;
            }
        }
    }
}
