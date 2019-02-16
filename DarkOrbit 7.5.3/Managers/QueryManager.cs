using MySQLManager.Database.Session_Details.Interfaces;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Ow.Game;
using Newtonsoft.Json;
using Ow.Game.Objects.Players;
using Ow.Game.Objects.Stations;
using Ow.Game.Clans;
using Ow.Game.Objects.Players.Managers;
using Ow.Game.Objects;
using Ow.Game.Objects.Collectables;
using Ow.Managers.MySQLManager;
using Ow.Game.Movements;
using static Ow.Game.Objects.Players.Managers.PlayerSettings;

namespace Ow.Managers
{
    class QueryManager
    {
        public class SavePlayer
        {
            public static void Settings(Player player)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE player_accounts SET settings = '{JsonConvert.SerializeObject(player.Settings)}' WHERE userID = {player.Id}");
                }
            }

            public static void Information(Player player)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE player_accounts SET Data = '{JsonConvert.SerializeObject(player.Data)}' WHERE userID = {player.Id}");
                }
            }
        }

        public static bool CheckSessionId(int userId, string sessionId)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var result = mySqlClient.ExecuteQueryRow($"SELECT sessionID FROM player_accounts WHERE userID = {userId}");
                return sessionId == result["sessionID"].ToString();
            }
        }

        public static void LoadUser(Player Player)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var data = mySqlClient.ExecuteQueryTable($"SELECT * FROM player_accounts WHERE userID = {Player.Id} ");
                    foreach (DataRow row in data.Rows)
                    {
                        Player.Level = Convert.ToInt32(row["level"]);
                        Player.Premium = Convert.ToBoolean(row["premium"]);
                        Player.Title = Convert.ToString(row["title"]);
                        Player.Clan = GameManager.GetClan(Convert.ToInt32(row["clanID"]));
                        Player.Ship = GameManager.GetShip(Convert.ToInt32(row["shipID"]));
                        Player.Data = JsonConvert.DeserializeObject<DataBase>(row["Data"].ToString());
                        Player.Settings = (row["settings"].ToString() == "" ? new PlayerSettings() : JsonConvert.DeserializeObject<PlayerSettings>(row["settings"].ToString()));
                    }

                    string sql = $"SELECT * FROM player_equipment WHERE userId = {Player.Id} ";
                    var querySet = mySqlClient.ExecuteQueryRow(sql);

                    Player.Equipment = JsonConvert.DeserializeObject<EquipmentBase>(querySet["configs"].ToString());
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("Failed getting player account [ID: " + Player.Id + "] " + e);
            }
        }

        public static Player GetPlayer(int playerId)
        {
            Player player = null;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    string sql = $"SELECT * FROM player_accounts WHERE userID = {playerId} ";
                    var querySet = mySqlClient.ExecuteQueryRow(sql);

                    var name = Convert.ToString(querySet["shipName"]);
                    var shipId = Convert.ToInt32(querySet["shipID"]);
                    var factionId = Convert.ToInt32(querySet["factionID"]);
                    var rankId = Convert.ToInt32(querySet["rankID"]);
                    var clan = GameManager.GetClan(Convert.ToInt32(querySet["clanID"]));

                    player = new Player(playerId, name, clan, factionId, new Position(0, 0), GameManager.GetSpacemap(0), rankId, shipId);
                }

            }
            catch (Exception e)
            {
                Out.WriteLine("Failed getting player account [ID: " + playerId + "] " + e);
            }
            return player;
        }

        public static void LoadMaps()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var data = (DataTable)mySqlClient.ExecuteQueryTable("SELECT * FROM server_maps");
                foreach (DataRow row in data.Rows)
                {
                    int MapID = Convert.ToInt32(row["mapID"]);
                    string Name = Convert.ToString(row["name"]);
                    int FactionID = Convert.ToInt32(row["factionID"]);
                    bool PvpMap = Convert.ToBoolean(row["isPvp"]);
                    bool StarterMap = Convert.ToBoolean(row["isStartedMap"]);
                    var Portals = JsonConvert.DeserializeObject<List<PortalBase>>(row["portals"].ToString());
                    var Stations = JsonConvert.DeserializeObject<List<StationBase>>(row["stations"].ToString());
                    var spacemap = new Spacemap(MapID, Name, FactionID, StarterMap, PvpMap, Portals, Stations);
                    GameManager.Spacemaps.TryAdd(spacemap.Id, spacemap);
                }
            }
        }

        public static void LoadShips()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var data = (DataTable)mySqlClient.ExecuteQueryTable("SELECT * FROM server_ships");
                foreach (DataRow row in data.Rows)
                {
                    string name = Convert.ToString(row["name"]);
                    int shipID = Convert.ToInt32(row["shipID"]);
                    int hitpoints = Convert.ToInt32(row["health"]);
                    string lootID = Convert.ToString(row["lootID"]);
                    var rewards = JsonConvert.DeserializeObject<ShipRewards>(row["reward"].ToString());

                    var ship = new Ship(name, shipID, hitpoints, lootID, rewards);
                    GameManager.Ships.TryAdd(ship.Id, ship);
                }
            }
        }

        public static void LoadClans()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var data = (DataTable)mySqlClient.ExecuteQueryTable("SELECT * FROM server_clan");
                foreach (DataRow row in data.Rows)
                {
                    int id = Convert.ToInt32(row["clanID"]);
                    string name = Convert.ToString(row["name"]);
                    string tag = Convert.ToString(row["tag"]);
                    int rankPoints = Convert.ToInt32(row["rankPoints"]);

                    var clan = new Clan(id, name, tag, rankPoints);
                    GameManager.Clans.TryAdd(clan.Id, clan);
                    LoadClanDiplomacy(clan);
                }
            }
        }

        private static void LoadClanDiplomacy(Clan clan)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var data = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM server_clan_diplomacy WHERE senderClan = {clan.Id}");
                foreach (DataRow row in data.Rows)
                {
                    int id = Convert.ToInt32(row["toClan"]);
                    Diplomacy relation = (Diplomacy)Convert.ToInt32(row["diplomacyType"]);
                    clan.Diplomacy.Add(id, relation);
                }

                var data2 = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM server_clan_diplomacy WHERE toClan = {clan.Id}");
                foreach (DataRow row in data2.Rows)
                {
                    int id = Convert.ToInt32(row["senderClan"]);
                    Diplomacy relation = (Diplomacy)Convert.ToInt32(row["diplomacyType"]);
                    clan.Diplomacy.Add(id, relation);
                }
            }
        }

        public static void LoadChatRooms()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var data = (DataTable)mySqlClient.ExecuteQueryTable("SELECT * FROM chat_rooms");
                foreach (DataRow row in data.Rows)
                {
                    int id = Convert.ToInt32(row["Id"]);
                    int index = Convert.ToInt32(row["Index"]);
                    string name = Convert.ToString(row["Name"]);

                    var chat = new Chat.Room(id, index, name);
                    Chat.Room.Rooms.Add(chat.Id, chat);
                }
            }
        }

        public static int GetChatPermission(int userId)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var data = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM chat_permissions WHERE UserId = {userId}");
                foreach (DataRow row in data.Rows)
                {
                    return Convert.ToInt32(row["Type"]);
                }
                return 0;
            }
        }
    }
}
