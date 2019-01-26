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
    abstract class Activatable
    {
        public const int ACTIVATED_RANGE = 500;

        public abstract byte[] GetAssetCreateCommand();

        public abstract void Click(GameSession gameSession);

        public abstract short GetAssetType();

        public Spacemap Spacemap { get; set; }
        public Position Position { get; set; }
        public Clan Clan { get; set; }
        public int Id { get; set; }
        public int FactionId { get; set; }

        public Activatable(Spacemap spacemap, int factionId, Position position, Clan clan)
        {
            Spacemap = spacemap;
            Id = Randoms.CreateRandomID();
            FactionId = factionId;
            Position = position;
            Clan = clan;
            Spacemap.Activatables.TryAdd(Id, this);
        }
    }
}
