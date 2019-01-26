using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    class Spectrum
    {
        public Player Player { get; set; }

        public bool Active = false;

        public Spectrum(Player player)
        {
            Player = player;
        }

        public void Tick()
        {
            if (Active)
            {
                if (cooldown.AddMilliseconds(TimeManager.SPECTRUM_DURATION) < DateTime.Now)
                    Disable();
            }
        }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            if (cooldown.AddMilliseconds(TimeManager.SPECTRUM_DURATION + TimeManager.SPECTRUM_COOLDOWN) < DateTime.Now || Player.GodMode)
            {
                Player.Spectrum = true;

                string packet = "0|SD|A|R|3|" + Player.Id + "";
                Player.SendPacket(packet);
                Player.SendPacketToInRangePlayers(packet);

                Player.SendCooldown(SkillManager.SPECTRUM, TimeManager.SPECTRUM_DURATION, true);
                Active = true;
                cooldown = DateTime.Now;
            }
        }

        public void Disable()
        {
            Player.Spectrum = false;

            string packet = "0|SD|D|R|3|" + Player.Id + "";
            Player.SendPacket(packet);
            Player.SendPacketToInRangePlayers(packet);

            Player.SendCooldown(SkillManager.SPECTRUM, TimeManager.SPECTRUM_COOLDOWN);
            Active = false;
        }
    }
}
