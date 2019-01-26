using Ow.Game.Clans;
using Ow.Game.Movements;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Objects.Stations
{
    abstract class Station
    {
        public const int ACTIVATED_RANGE = 500;

        public abstract string GetAssetCreatePacket();

        public Spacemap Spacemap { get; set; }
        public Position Position { get; set; }
        public int Id { get; set; }
        public int FactionId { get; set; }

        public Station(Spacemap spacemap, int factionId, Position position)
        {
            Spacemap = spacemap;
            Id = Randoms.CreateRandomID();
            FactionId = factionId;
            Position = position;
            Spacemap.Stations.TryAdd(Id, this);
        }
    }
}
