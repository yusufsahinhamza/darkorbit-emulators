using Ow.Game.Objects;
using Ow.Managers;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game.Events
{
    class Spaceball
    {
        public Objects.Spaceball Character { get; set; }

        public bool Active = false;
        public int Type = Ship.SPACEBALL_SUMMER;
        public List<Portal> Portals = new List<Portal>();
        public int Limit = 20;

        public void Start()
        {
            if (Active) return;
            GameManager.SendPacketToAll("0|A|STD|Spaceball event started!");
            Character = new Objects.Spaceball(Randoms.CreateRandomID(), Type);

            Portals.Add(new Portal(Character.Spacemap, Character.MMOPosition, null, 0, 62, 0, true, false));
            Portals.Add(new Portal(Character.Spacemap, Character.EICPosition, null, 0, 61, 0, true, false));
            Portals.Add(new Portal(Character.Spacemap, Character.VRUPosition, null, 0, 61, 0, true, false));

            Active = true;

            foreach (var gameSession in GameManager.GameSessions.Values)
            {
                var player = gameSession.Player;
                player.SettingsManager.SendMenuBarsCommand();

                foreach (var portal in Portals)
                    GameManager.SendCommandToMap(Character.Spacemap.Id, portal.GetAssetCreateCommand());
            }

            Character.Spacemap.AddCharacter(Character);

            Program.TickManager.AddTick(Character);
        }

        public void Stop()
        {
            if (!Active) return;
            GameManager.SendPacketToAll("0|A|STD|Spaceball event ended!");
            Active = false;
            Limit = 20;

            foreach (var gameSession in GameManager.GameSessions.Values)
            {
                var player = gameSession.Player;
                player.SettingsManager.SendMenuBarsCommand();
            }

            foreach (var portal in Portals)
                portal.Remove();

            Character.Spacemap.RemoveCharacter(Character);
            Program.TickManager.RemoveTick(Character);
        }
    }
}
