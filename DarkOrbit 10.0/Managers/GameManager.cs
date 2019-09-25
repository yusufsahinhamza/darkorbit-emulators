using Ow.Chat;
using Ow.Game;
using Ow.Game.Events;
using Ow.Game.Objects;
using Ow.Game.Objects.Players;
using Ow.Game.Objects.Stations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ow.Game.GameSession;

namespace Ow.Managers
{
    class GameManager
    {
        public static ConcurrentDictionary<int, ChatClient> ChatClients = new ConcurrentDictionary<int, ChatClient>();
        public static ConcurrentDictionary<int, GameSession> GameSessions = new ConcurrentDictionary<int, GameSession>();
        public static ConcurrentDictionary<int, Spacemap> Spacemaps = new ConcurrentDictionary<int, Spacemap>();
        public static ConcurrentDictionary<string, BattleStation> BattleStations = new ConcurrentDictionary<string, BattleStation>();
        public static ConcurrentDictionary<int, Ship> Ships = new ConcurrentDictionary<int, Ship>();
        public static ConcurrentDictionary<int, Clan> Clans = new ConcurrentDictionary<int, Clan>();
        public static List<Group> Groups = new List<Group>();

        public async static void Restart(int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                var packet = $"0|A|STM|server_restart_n_seconds|%!|{i}";
                SendPacketToAll(packet);
                await Task.Delay(1000);

                if (i <= 1)
                {
                    foreach (var gameSession in GameSessions.Values)
                        gameSession.Disconnect(DisconnectionType.NORMAL);

                    foreach (var battleStation in BattleStations.Values)
                    {
                        QueryManager.BattleStations.BattleStation(battleStation);
                        QueryManager.BattleStations.Modules(battleStation);
                    }

                    Environment.Exit(0);
                }
            }
        }

        public static void SendChatSystemMessage(string message)
        {
            foreach (var chat in ChatClients.Values)
                chat.Send($"dq%{message}#");
        }

        public static void SendCommandToMap(int mapId, byte[] command)
        {
            var spacemap = GetSpacemap(mapId);

            if (spacemap != null && command != null)
            {
                foreach (var player in spacemap.Characters.Values)
                    if (player.Spacemap == spacemap)
                        if(player is Player)
                            (player as Player).SendCommand(command);
            }
        }

        public static void SendPacketToMap(int mapId, string packet)
        {
            var spacemap = GetSpacemap(mapId);

            if (spacemap != null && packet != null)
            {
                foreach (var player in spacemap.Characters.Values)
                    if (player.Spacemap == spacemap)
                        if(player is Player)
                            (player as Player).SendPacket(packet);
            }
        }

        public static void SendCommandToAll(byte[] command)
        {
            foreach (var gameSession in GameSessions.Values)
            {
                var player = gameSession.Player;
                player.SendCommand(command);
            }
        }

        public static void SendPacketToAll(string packet)
        {
            foreach (var gameSession in GameSessions.Values)
            {
                var player = gameSession.Player;
                player.SendPacket(packet);
            }
        }

        public static void SendCommandToUser(int userId, byte[] command)
        {
            var player = GetPlayerById(userId);
            player.SendCommand(command);
        }

        public static void SendPacketToUser(int userId, string packet)
        {
            var player = GetPlayerById(userId);
            player.SendPacket(packet);
        }

        public static GameSession GetGameSession(int UserID)
        {
            return !GameSessions.ContainsKey(UserID) ? null : GameSessions[UserID];
        }

        public static Player GetPlayerById(int id)
        {
            return GameSessions.FirstOrDefault(x => x.Value.Player.Id == id).Value?.Player;
        }

        public static Player GetPlayerByName(string name)
        {
            return GameSessions.FirstOrDefault(x => x.Value.Player.Name == name).Value?.Player;
        }

        public static Spacemap GetSpacemap(int MapID)
        {
            return !Spacemaps.ContainsKey(MapID) ? null : Spacemaps[MapID];
        }

        public static Ship GetShip(int ShipID)
        {
            return !Ships.ContainsKey(ShipID) ? null : Ships[ShipID];
        }

        public static Clan GetClan(int clanId)
        {
            return !Clans.ContainsKey(clanId) ? null : Clans[clanId];
        }
    }
}
