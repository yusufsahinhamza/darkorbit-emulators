using Ow.Game.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game
{
    abstract class Object
    {
        public int Id { get; }
        public Position Position { get; set; }
        public Spacemap Spacemap { get; set; }
        public int Range { get; set; }

        public Object(int id, Position pos, Spacemap map, int range = 1000)
        {
            Id = id;
            Position = pos;
            Spacemap = map;
            Range = range;

            Spacemap.Objects.TryAdd(Id, this);
        }
    }
}
