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
using Ow.Game.Objects.Players.Managers;
using Ow.Game.Objects;
using Ow.Game.Objects.Collectables;
using Ow.Managers.MySQLManager;
using Ow.Game.Movements;
using Newtonsoft.Json.Linq;
using Ow.Net;
using Ow.Net.netty.commands;

namespace Ow.Managers
{
    class QueryManager
    {
        public class SavePlayer
        {
            public static void Settings(Player player, string target, object settings)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                    mySqlClient.ExecuteNonQuery($"UPDATE player_settings SET {target} = '{JsonConvert.SerializeObject(settings)}' WHERE userId = {player.Id}");
            }

            public static void Information(Player player)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                    mySqlClient.ExecuteNonQuery($"UPDATE player_accounts SET Data = '{JsonConvert.SerializeObject(player.Data)}', level = {player.Level} WHERE userID = {player.Id}");
            }

            public static void Boosters(Player player)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                    mySqlClient.ExecuteNonQuery($"UPDATE player_equipment SET boosters = '{JsonConvert.SerializeObject(player.BoosterManager.Boosters)}' WHERE userId = {player.Id}");
            }
        }

        public class ChatFunctions
        {
            public static void AddBan(int bannedId, int modId, string reason, int typeId, string untilDate)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    mySqlClient.ExecuteNonQuery($"INSERT INTO server_bans (userId, modId, reason, typeId, until_date) VALUES ({bannedId}, {modId}, '{reason}', {typeId}, '{untilDate}')");
                }
            }

            public static bool Banned(int userId)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var result = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT id FROM server_bans WHERE userId = {userId} AND typeId = 0");
                    return result.Rows.Count >= 1 ? true : false;
                }
            }
        }

        public static string GetUserShipName(int userId)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var result = mySqlClient.ExecuteQueryRow($"SELECT shipName FROM player_accounts WHERE userID = {userId}");
                return result["shipName"].ToString();
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

        public static bool Banned(int userId)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var result = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT id FROM server_bans WHERE userId = {userId} AND typeId = 1");
                return result.Rows.Count >= 1 ? true : false;
            }
        }

        public static void LoadUser(Player Player)
        {
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var data = mySqlClient.ExecuteQueryTable($"SELECT * FROM player_accounts WHERE userID = {Player.Id}");
                    foreach (DataRow row in data.Rows)
                    {
                        Player.Level = Convert.ToInt32(row["level"]);
                        Player.Premium = Convert.ToBoolean(row["premium"]);
                        Player.Title = Convert.ToString(row["title"]);
                        Player.Clan = GameManager.GetClan(Convert.ToInt32(row["clanID"]));
                        Player.Ship = GameManager.GetShip(Convert.ToInt32(row["shipID"]));
                        Player.Data = JsonConvert.DeserializeObject<DataBase>(row["Data"].ToString());
                    }

                    string settingsSql = $"SELECT * FROM player_settings WHERE userId = {Player.Id} ";
                    var settingsResult = mySqlClient.ExecuteQueryRow(settingsSql);

                    if (settingsResult["audio"].ToString() != "") Player.Settings.Audio = JsonConvert.DeserializeObject<AudioBase>(settingsResult["audio"].ToString());
                    if (settingsResult["quality"].ToString() != "") Player.Settings.Quality = JsonConvert.DeserializeObject<QualityBase>(settingsResult["quality"].ToString());
                    if (settingsResult["classY2T"].ToString() != "") Player.Settings.ClassY2T = JsonConvert.DeserializeObject<ClassY2TBase>(settingsResult["classY2T"].ToString());
                    if (settingsResult["display"].ToString() != "") Player.Settings.Display = JsonConvert.DeserializeObject<DisplayBase>(settingsResult["display"].ToString());
                    if (settingsResult["gameplay"].ToString() != "") Player.Settings.Gameplay = JsonConvert.DeserializeObject<GameplayBase>(settingsResult["gameplay"].ToString());
                    if (settingsResult["window"].ToString() != "") Player.Settings.Window = JsonConvert.DeserializeObject<WindowBase>(settingsResult["window"].ToString());
                    if (settingsResult["inGameSettings"].ToString() != "") Player.Settings.InGameSettings = JsonConvert.DeserializeObject<InGameSettingsBase>(settingsResult["inGameSettings"].ToString());
                    if (settingsResult["cooldowns"].ToString() != "") Player.Settings.Cooldowns = JsonConvert.DeserializeObject<Dictionary<string, int>>(settingsResult["cooldowns"].ToString());
                    if (settingsResult["boundKeys"].ToString() != "") Player.Settings.BoundKeys = JsonConvert.DeserializeObject<List<BoundKeysBase>>(settingsResult["boundKeys"].ToString());
                    if (settingsResult["slotbarItems"].ToString() != "") Player.Settings.SlotBarItems = JsonConvert.DeserializeObject<Dictionary<short, string>>(settingsResult["slotbarItems"].ToString());
                    if (settingsResult["premiumSlotbarItems"].ToString() != "") Player.Settings.PremiumSlotBarItems = JsonConvert.DeserializeObject<Dictionary<short, string>>(settingsResult["premiumSlotbarItems"].ToString());
                    if (settingsResult["proActionBarItems"].ToString() != "") Player.Settings.ProActionBarItems = JsonConvert.DeserializeObject<Dictionary<short, string>>(settingsResult["proActionBarItems"].ToString());

                    string sql = $"SELECT * FROM player_equipment WHERE userId = {Player.Id} ";
                    var querySet = mySqlClient.ExecuteQueryRow(sql);

                    Player.BoosterManager.Boosters = JsonConvert.DeserializeObject<Dictionary<short, List<BoosterBase>>>(querySet["boosters"].ToString());

                    Player.Equipment = querySet["configs"].ToString() != "" ? JsonConvert.DeserializeObject<EquipmentBase>(querySet["configs"].ToString()) : new EquipmentBase(Player.Ship.BaseHitpoints, 0, 0, 300, Player.Ship.BaseHitpoints, 0, 0, 300); //TODO: GGA (i dont remember what i said )
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

                    player = new Player(playerId, name, clan, factionId, rankId, GameManager.GetShip(shipId));
                    player.Pet.Name = Convert.ToString(querySet["petName"]);
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
                    int mapId = Convert.ToInt32(row["mapID"]);
                    string name = Convert.ToString(row["name"]);
                    int factionId = Convert.ToInt32(row["factionID"]);
                    var portals = JsonConvert.DeserializeObject<List<PortalBase>>(row["portals"].ToString());
                    var stations = JsonConvert.DeserializeObject<List<StationBase>>(row["stations"].ToString());
                    var options = JsonConvert.DeserializeObject<OptionsBase>(row["options"].ToString());
                    var spacemap = new Spacemap(mapId, name, factionId, portals, stations, options);
                    GameManager.Spacemaps.TryAdd(spacemap.Id, spacemap);
                }
            }

            LoadBattleStations();
        }


        public class BattleStations
        {
            public static void BattleStation(BattleStation battleStation)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var visualModifiers = new List<int>();

                    foreach (var modifier in battleStation.VisualModifiers.Keys)
                        visualModifiers.Add(modifier);

                    var buildTime = battleStation.AssetTypeId != AssetTypeModule.BATTLESTATION && battleStation.InBuildingState ? $"buildTime = '{battleStation.buildTime.ToString("yyyy-MM-dd HH:mm:ss")}'," : "";

                    mySqlClient.ExecuteNonQuery($"UPDATE server_battlestations SET clanId = {battleStation.Clan.Id}, factionId = {battleStation.FactionId}" +
                    $", inBuildingState = {battleStation.InBuildingState}, buildTimeInMinutes = {battleStation.BuildTimeInMinutes}, {buildTime}" +
                    $"visualModifiers = '{JsonConvert.SerializeObject(visualModifiers)}' WHERE name = '{battleStation.AsteroidName}'");
                }
            }

            public static void Modules(BattleStation battleStation)
            {
                var modules = new List<EquippedModuleBase>();

                foreach (var equipped in battleStation.EquippedStationModule)
                {
                    var module = new List<SatelliteBase>();

                    foreach (var equippedModule in battleStation.EquippedStationModule[equipped.Key])
                    {
                        module.Add(new SatelliteBase(equippedModule.OwnerId, equippedModule.ItemId, equippedModule.SlotId, equippedModule.DesignId, equippedModule.Type, equippedModule.CurrentHitPoints,
                            equippedModule.MaxHitPoints, equippedModule.CurrentShieldPoints, equippedModule.MaxShieldPoints, equippedModule.InstallationSecondsLeft, equippedModule.Installed));
                    }

                    modules.Add(new EquippedModuleBase(equipped.Key, module));
                }

                using (var mySqlClient = SqlDatabaseManager.GetClient())
                    mySqlClient.ExecuteNonQuery($"UPDATE server_battlestations SET modules = '{JsonConvert.SerializeObject(modules)}' WHERE name = '{battleStation.AsteroidName}'");
            }
        }

        public static void LoadBattleStations()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var data = (DataTable)mySqlClient.ExecuteQueryTable("SELECT * FROM server_battlestations");
                foreach (DataRow row in data.Rows)
                {
                    string name = Convert.ToString(row["name"]);
                    int mapId = Convert.ToInt32(row["mapId"]);
                    int clanId = Convert.ToInt32(row["clanId"]);
                    int factionId = Convert.ToInt32(row["factionId"]);
                    int positionX = Convert.ToInt32(row["positionX"]);
                    int positionY = Convert.ToInt32(row["positionY"]);
                    var modules = JsonConvert.DeserializeObject<List<EquippedModuleBase>>(row["modules"].ToString());
                    var inBuildingState = Convert.ToBoolean(Convert.ToInt32(row["inBuildingState"]));
                    var buildTimeInMinutes = Convert.ToInt32(row["buildTimeInMinutes"]);
                    var buildTime = DateTime.Parse(row["buildTime"].ToString());
                    var visualModifiers = JsonConvert.DeserializeObject<List<int>>(row["visualModifiers"].ToString());

                    var battleStation = new BattleStation(name, factionId, GameManager.GetSpacemap(mapId), new Position(positionX, positionY), GameManager.GetClan(clanId), modules, inBuildingState, buildTimeInMinutes, buildTime, visualModifiers);
                    GameManager.BattleStations.TryAdd(battleStation.Name, battleStation);
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
            GameManager.Clans.TryAdd(0, new Clan(0, "", "", 0, 0));
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var data = (DataTable)mySqlClient.ExecuteQueryTable("SELECT * FROM server_clan");
                foreach (DataRow row in data.Rows)
                {
                    int id = Convert.ToInt32(row["clanID"]);
                    string name = Convert.ToString(row["name"]);
                    string tag = Convert.ToString(row["tag"]);
                    int rankPoints = Convert.ToInt32(row["rankPoints"]);
                    int factionId = Convert.ToInt32(row["factionID"]);

                    var clan = new Clan(id, name, tag, factionId, rankPoints);
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
                    clan.Diplomacies.Add(id, relation);
                }

                var data2 = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM server_clan_diplomacy WHERE toClan = {clan.Id}");
                foreach (DataRow row in data2.Rows)
                {
                    int id = Convert.ToInt32(row["senderClan"]);
                    Diplomacy relation = (Diplomacy)Convert.ToInt32(row["diplomacyType"]);
                    clan.Diplomacies.Add(id, relation);
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
