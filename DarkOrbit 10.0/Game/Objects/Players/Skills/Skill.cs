using Ow.Game.Ticks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Players.Skills
{
    abstract class Skill : Tick
    {
        public int TickId { get; set; }
        public abstract string LootId { get; }
        public abstract int Id { get; }

        public abstract int Duration { get; }
        public abstract int Cooldown { get; }

        public Player Player { get; set; }

        public bool Active = false;
        public DateTime cooldown = new DateTime();

        public Skill(Player player)
        {
            Player = player;

            var tickId = -1;
            Program.TickManager.AddTick(this, out tickId);
            TickId = tickId;
        }

        public abstract void Tick();
        public abstract void Send();
        public abstract void Disable();
    }
}
