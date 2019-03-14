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

        public ConcurrentDictionary<int, Tick> Ticks = new ConcurrentDictionary<int, Tick>();

        private int GetNextTickId()
        {
            var i = 0;
            while (true)
            {
                if (Ticks.ContainsKey(i))
                    i++;
                else return i;
            }
        }

        public void AddTick(Tick tick, out int id)
        {
            id = -1;
            if (Ticks.Values.Contains(tick)) return;

            id = GetNextTickId();
            Ticks.TryAdd(id, tick);
        }

        public void RemoveTick(Tick tick)
        {
            if (!Ticks.ContainsKey(tick.TickId)) return;

            Ticks.TryRemove(tick.TickId, out tick);
        }

        public bool Exists(Tick tickable)
        {
            if (Ticks.Count == 0) return false;
            if (Ticks.ContainsKey(tickable.TickId)) return true;
            return false;
        }

        public async void Tick()
        {
            while (true)
            {
                foreach (var tickable in Ticks)
                {
                    Task.Factory.StartNew(tickable.Value.Tick);
                }
                await Task.Delay(TICKS_PER_SECOND);
            }
        }
    }
}
