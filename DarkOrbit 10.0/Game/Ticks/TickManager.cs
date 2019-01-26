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
        public static short TICKS_PER_SECOND = 64;

        public ConcurrentDictionary<int, Tick> Ticks = new ConcurrentDictionary<int, Tick>();

        private int GetNextTickId()
        {
            using (var enumerator = Ticks.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return 0;

                var nextKeyInSequence = enumerator.Current.Key + 1;

                if (nextKeyInSequence < 1)
                    throw new InvalidOperationException("The dictionary contains keys less than 0");

                if (nextKeyInSequence != 1)
                    return 0;

                while (enumerator.MoveNext())
                {
                    var key = enumerator.Current.Key;
                    if (key > nextKeyInSequence)
                        return nextKeyInSequence;

                    ++nextKeyInSequence;
                }

                return nextKeyInSequence;
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
