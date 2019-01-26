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

namespace Ow.Managers
{
    class QueryManager
    {
        public class SavePlayer
        {
            public static void Settings(Player Player)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE players_accounts SET settings = '{Player.Settings.ToString()}' WHERE userID = {Player.Id}");
                }
            }

            public static void Information(Player Player)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"UPDATE players_accounts SET " +
                    $"uridium = {Player.Uridium}," +
                    $"credits = {Player.Credits}," +
                    $"honor = {Player.Honor}," +
                    $"experience = {Player.Experience}" +
                    $" WHERE userID =" + Player.Id);
                }
            }
        }

        public static void LoadUser(Player Player)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var data = mySqlClient.ExecuteQueryTable($"SELECT * FROM players_accounts WHERE userID = {Player.Id} ");
                    foreach (DataRow row in data.Rows)
                    {
                        Player.Uridium = Convert.ToInt32(row["uridium"]);
                        Player.Experience = Convert.ToInt32(row["experience"]);
                        Player.Credits = Convert.ToInt32(row["credits"]);
                        Player.Level = Convert.ToInt32(row["level"]);
                        Player.Honor = Convert.ToInt32(row["honor"]);
                        Player.Premium = Convert.ToBoolean(row["premium"]);
                        Player.Title = Convert.ToString(row["title"]);
                        Player.Clan = GameManager.GetClan(Convert.ToInt32(row["clanID"]));
                        Player.Ship = GameManager.GetShip(Convert.ToInt32(row["shipID"]));
                        Player.Settings = JsonConvert.DeserializeObject<PlayerSettings>(row["settings"].ToString());
                    }
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
                    string sql = $"SELECT * FROM players_accounts WHERE userID = {playerId} ";
                    var querySet = mySqlClient.ExecuteQueryRow(sql);

                    var name = Convert.ToString(querySet["shipName"]);
                    var shipId = Convert.ToInt32(querySet["shipID"]);
                    var ship = GameManager.GetShip(Convert.ToInt32(querySet["shipID"]));
                    var mapId = Convert.ToInt32(querySet["mapID"]);

                    if (!GameManager.Spacemaps.ContainsKey(mapId))
                    {
                        Console.WriteLine("PROBLEM -> MAPID " + mapId + " DOESN'T EXIST!");
                        return null;
                    }

                    var spacemap = GameManager.GetSpacemap(mapId);
                    //var currentHealth = intConv(querySet["SHIP_HP"]);
                    //var currentNanohull = intConv(querySet["SHIP_NANO"]);
                    var factionId = Convert.ToInt32(querySet["factionID"]);
                    var rankId = Convert.ToInt32(querySet["rankID"]);
                    //var sessionId = stringConv(querySet["SESSION_ID"]);
                    var clan = GameManager.GetClan(Convert.ToInt32(querySet["clanID"]));
                    //var clan = playerId == 5036 ? Global.StorageManager.Clans[2] : Global.StorageManager.Clans[1];

                    player = new Player(playerId, name, clan, factionId, new Position(0, 0), spacemap, rankId, shipId);
                }

            }
            catch (Exception e)
            {
                Out.WriteLine("Failed getting player account [ID: " + playerId + "] " + e);
            }
            return player;
        }

        public static DataTable data { get; set; }
        public static int LoadItem(int UserId, string lootID, uint ConfigID, bool isDrone)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                if (isDrone)
                    data = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM players_equipment WHERE lootID = {lootID} AND configID = {ConfigID}  AND playerID = {UserId}");
                else
                    data = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM players_equipment WHERE lootID = {lootID} AND configID= {ConfigID}  AND playerID = {UserId}");

                if (isDrone ? lootID == "equipment_drones_weapon_shield_b0_2" : lootID == "equipment_weapon_shield_b0_2")
                {
                    return isDrone ? data.Rows.Count * 17500 : data.Rows.Count * 10000;
                }
                else if (isDrone ? lootID == "equipment_drones_weapon_laser_lf_3" : lootID == "equipment_weapon_laser_lf_3")
                {
                    return isDrone ? data.Rows.Count * 200 : data.Rows.Count * 150;
                }
                else return 0;
            }
        }

        public static void LoadMaps()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var data = (DataTable)mySqlClient.ExecuteQueryTable("SELECT * FROM maps");
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
                var data = (DataTable)mySqlClient.ExecuteQueryTable("SELECT * FROM ships");
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
                var data = (DataTable)mySqlClient.ExecuteQueryTable("SELECT * FROM clans");
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
                var data = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM clans_diplomacy WHERE clanID1 = {clan.Id}");
                foreach (DataRow row in data.Rows)
                {
                    int id = Convert.ToInt32(row["clanID2"]);
                    Diplomacy relation = (Diplomacy)Convert.ToInt32(row["relationType"]);
                    clan.Diplomacy.Add(id, relation);
                }

                var data2 = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM clans_diplomacy WHERE clanID2 = {clan.Id}");
                foreach (DataRow row in data2.Rows)
                {
                    int id = Convert.ToInt32(row["clanID1"]);
                    Diplomacy relation = (Diplomacy)Convert.ToInt32(row["relationType"]);
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
