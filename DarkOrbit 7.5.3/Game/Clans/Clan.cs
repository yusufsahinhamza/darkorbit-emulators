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

        public Dictionary<int, Diplomacy> Diplomacy { get; set; }

        public Dictionary<int, ClanMember> Members { get; set; }

        public Clan(int id, string name, string tag, int rankPoints)
        {
            Id = id;
            Name = name;
            Tag = tag;
            Diplomacy = new Dictionary<int, Diplomacy>();
            Members = new Dictionary<int, ClanMember>();
            RankPoints = rankPoints;
            AddMembers();
        }

        public void AddMembers()
        {
            foreach(var gameSession in GameManager.GameSessions.Values)
            {
                if (gameSession.Player == null) return;
                if (gameSession.Player.Clan != this) return;

                var member = new ClanMember(gameSession.Player);
                Members.Add(gameSession.Player.Id, member);
            }
        }

        public short GetRelation(Clan clan)
        {
            if (clan == this && clan.Id != 0)
                return (short)Game.Diplomacy.ALLIED;
            if (Diplomacy.ContainsKey(clan.Id))
                return (short)Diplomacy[clan.Id];
            return (short)Game.Diplomacy.NONE;
        }
    }
}
