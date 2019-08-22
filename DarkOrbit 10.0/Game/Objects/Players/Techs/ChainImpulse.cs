using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Techs
{
    class ChainImpulse
    {
        public Player Player { get; set; }

        public static int DAMAGE = 10000;

        public ChainImpulse(Player player) { Player = player; }

        public DateTime cooldown = new DateTime();
        public void Send()
        {
            /*
            if (cooldown.AddMilliseconds(TimeManager.CHAIN_IMPULSE_COOLDOWN) < DateTime.Now || Player.Storage.GodMode)
            {
                Player.AttackManager.ECI();
                Player.SendCooldown(TechManager.TECH_CHAIN_IMPULSE, TimeManager.CHAIN_IMPULSE_COOLDOWN);
                cooldown = DateTime.Now;
            }
            */
        }
    }
}
