using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Ow.Game.Ticks
{
    class TickManager
    {
        public static short TICKS_PER_SECOND = 84;

        public List<Tick> Ticks = new List<Tick>();

        public void AddTick(Tick tick)
        {
            if (!Ticks.Contains(tick))
                Ticks.Add(tick);
        }

        public void RemoveTick(Tick tick)
        {
            if (Ticks.Contains(tick))
                Ticks.Remove(tick);
        }

        public bool Exists(Tick tickable)
        {
            if (Ticks.Count == 0) return false;
            if (Ticks.Contains(tickable)) return true;
            return false;
        }

        public async void Tick()
        {
            while (true)
            {       
                for (var i = 0; i < Ticks.Count; i++)
                    Ticks[i].Tick();

                await Task.Delay(TICKS_PER_SECOND);
            }
        }
    }
}
