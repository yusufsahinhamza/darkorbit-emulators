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

        public void Start()
        {
            GameManager.SendPacketToAll("0|A|STD|Spaceball started!");
            Character = new Objects.Spaceball(Randoms.CreateRandomID(), Type);

            Portals.Add(new Portal(Character.Spacemap, Character.MMOPosition, null, 0, 62, 0, true, false));
            Portals.Add(new Portal(Character.Spacemap, Character.EICPosition, null, 0, 61, 0, true, false));
            Portals.Add(new Portal(Character.Spacemap, Character.VRUPosition, null, 0, 61, 0, true, false));

            Active = true;
            foreach (var gameSession in GameManager.GameSessions.Values)
            {
                var player = gameSession.Player;
                player.SettingsManager.SendRemoveWindows();

                if (player.Spacemap == Character.Spacemap)
                    foreach (var portal in Portals)
                        player.SendPacket(portal.GetAssetCreatePacket());
            }
            Character.Spacemap.AddCharacter(Character);

            var tickId = -1;
            Program.TickManager.AddTick(Character, out tickId);
            Character.TickId = tickId;
        }

        public void Stop()
        {
            GameManager.SendPacketToAll("0|A|STD|Spaceball stopped!");
            Active = false;
            foreach (var gameSession in GameManager.GameSessions.Values)
            {
                var player = gameSession.Player;
                player.SettingsManager.SendRemoveWindows();
            }

            foreach (var portal in Portals)
                portal.Remove();

            Character.Spacemap.RemoveCharacter(Character);
            Program.TickManager.RemoveTick(Character);
        }
    }
}
