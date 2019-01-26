using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Sentinel
    {
        public Player Player { get; set; }

        public bool Active = false;

        public Sentinel(Player player)
        {
            Player = player;
        }

        public void Tick()
        {
            if (Active)
                if (cooldown.AddMilliseconds(TimeManager.SENTINEL_DURATION) < DateTime.Now)
                    Disable();
        }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (cooldown.AddMilliseconds(TimeManager.SENTINEL_DURATION + TimeManager.SENTINEL_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                Player.Sentinel = true;

                string packet = "0|SD|A|R|4|" + Player.Id + "";
                Player.SendPacket(packet);
                Player.SendPacketToInRangePlayers(packet);

                Player.SendCooldown(SkillManager.SENTINEL, TimeManager.SENTINEL_DURATION, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public void Disable()
        {
            Player.Sentinel = false;

            string packet = "0|SD|D|R|4|" + Player.Id + "";
            Player.SendPacket(packet);
            Player.SendPacketToInRangePlayers(packet);

            Player.SendCooldown(SkillManager.SENTINEL, TimeManager.SENTINEL_COOLDOWN);
            Active = false;
        }
    }
}
