using Ow.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Clans
{
    class Clan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public int RankPoints { get; set; }

        public Dictionary<int, Diplomacy> Diplomacies = new Dictionary<int, Diplomacy>();

        public Clan(int id, string name, string tag, int rankPoints)
        {
            Id = id;
            Name = name;
            Tag = tag;
            RankPoints = rankPoints;
        }

        public short GetRelation(Clan clan)
        {
            if (clan == this && clan.Id != 0)
                return (short)Game.Diplomacy.ALLIED;
            if (Diplomacies.ContainsKey(clan.Id))
                return (short)Diplomacies[clan.Id];
            return (short)Game.Diplomacy.NONE;
        }
    }
}
