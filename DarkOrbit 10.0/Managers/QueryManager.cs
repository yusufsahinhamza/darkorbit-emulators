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
                    mySqlClient.ExecuteNonQuery($"UPDATE player_accounts SET Data = '{JsonConvert.SerializeObject(player.Data)}', level = {player.Level}, nanohull = {player.CurrentNanoHull}  WHERE userID = {player.Id}");
            }

            public static void Boosters(Player player)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                    mySqlClient.ExecuteNonQuery($"UPDATE player_equipment SET boosters = '{JsonConvert.SerializeObject(player.BoosterManager.Boosters)}' WHERE userId = {player.Id}");
            }

            public static void Modules(Player player)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                    mySqlClient.ExecuteNonQuery($"UPDATE player_equipment SET modules = '{JsonConvert.SerializeObject(player.Storage.BattleStationModules)}' WHERE userId = {player.Id}");
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

        public static Player GetPlayer(int playerId)
        {
            Player player = null;
            try
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var data = mySqlClient.ExecuteQueryTable($"SELECT * FROM player_accounts WHERE userID = {playerId}");
                    foreach (DataRow row in data.Rows)
                    {
                        var name = Convert.ToString(row["shipName"]);
                        var ship = GameManager.GetShip(Convert.ToInt32(row["shipID"]));
                        var factionId = Convert.ToInt32(row["factionID"]);
                        var rankId = Convert.ToInt32(row["rankID"]);
                        var clan = GameManager.GetClan(Convert.ToInt32(row["clanID"]));

                        player = new Player(playerId, name, clan, factionId, rankId, ship);
                        player.Pet.Name = Convert.ToString(row["petName"]);
                        player.Level = Convert.ToInt32(row["level"]);
                        player.Premium = Convert.ToBoolean(row["premium"]);
                        player.Title = Convert.ToString(row["title"]);
                        player.Data = JsonConvert.DeserializeObject<DataBase>(row["Data"].ToString());
                        player.CurrentNanoHull = Convert.ToInt32(row["nanohull"]);
                    }

                    var skill = mySqlClient.ExecuteQueryTable($"SELECT * FROM player_skilltree WHERE userID = {playerId}");
                    foreach (DataRow row in skill.Rows)
                    {
                        player.SkillTree.Engineering = Convert.ToInt32(row["skill_13"]);
                        player.SkillTree.Detonation1 = Convert.ToInt32(row["skill_5a"]);
                        player.SkillTree.Detonation2 = Convert.ToInt32(row["skill_5b"]);
                        player.SkillTree.HeatseekingMissiles = Convert.ToInt32(row["skill_20"]);
                        player.SkillTree.RocketFusion = Convert.ToInt32(row["skill_6"]);
                        player.SkillTree.Cruelty1 = Convert.ToInt32(row["skill_21a"]);
                        player.SkillTree.Cruelty2 = Convert.ToInt32(row["skill_21b"]);
                        player.SkillTree.Explosives = Convert.ToInt32(row["skill_1"]);
                    }

                    var settings = mySqlClient.ExecuteQueryTable($"SELECT * FROM player_settings WHERE userId = {playerId}");
                    foreach (DataRow row in settings.Rows)
                    {
                        if (row["audio"].ToString() != "")
                            player.Settings.Audio = JsonConvert.DeserializeObject<AudioBase>(row["audio"].ToString());
                        if (row["quality"].ToString() != "")
                            player.Settings.Quality = JsonConvert.DeserializeObject<QualityBase>(row["quality"].ToString());
                        if (row["classY2T"].ToString() != "")
                            player.Settings.ClassY2T = JsonConvert.DeserializeObject<ClassY2TBase>(row["classY2T"].ToString());
                        if (row["display"].ToString() != "")
                            player.Settings.Display = JsonConvert.DeserializeObject<DisplayBase>(row["display"].ToString());
                        if (row["gameplay"].ToString() != "")
                            player.Settings.Gameplay = JsonConvert.DeserializeObject<GameplayBase>(row["gameplay"].ToString());
                        if (row["window"].ToString() != "")
                            player.Settings.Window = JsonConvert.DeserializeObject<WindowBase>(row["window"].ToString());
                        if (row["inGameSettings"].ToString() != "")
                            player.Settings.InGameSettings = JsonConvert.DeserializeObject<InGameSettingsBase>(row["inGameSettings"].ToString());
                        if (row["cooldowns"].ToString() != "")
                            player.Settings.Cooldowns = JsonConvert.DeserializeObject<Dictionary<string, string>>(row["cooldowns"].ToString());
                        if (row["boundKeys"].ToString() != "")
                            player.Settings.BoundKeys = JsonConvert.DeserializeObject<List<BoundKeysBase>>(row["boundKeys"].ToString());
                        if (row["slotbarItems"].ToString() != "")
                            player.Settings.SlotBarItems = JsonConvert.DeserializeObject<Dictionary<short, string>>(row["slotbarItems"].ToString());
                        if (row["premiumSlotbarItems"].ToString() != "")
                            player.Settings.PremiumSlotBarItems = JsonConvert.DeserializeObject<Dictionary<short, string>>(row["premiumSlotbarItems"].ToString());
                        if (row["proActionBarItems"].ToString() != "")
                            player.Settings.ProActionBarItems = JsonConvert.DeserializeObject<Dictionary<short, string>>(row["proActionBarItems"].ToString());
                    }

                    var equipment = mySqlClient.ExecuteQueryTable($"SELECT * FROM player_equipment WHERE userId = {playerId}");
                    foreach (DataRow row in equipment.Rows)
                    {
                        player.BoosterManager.Boosters = JsonConvert.DeserializeObject<Dictionary<short, List<BoosterBase>>>(row["boosters"].ToString());
                        player.Storage.BattleStationModules = JsonConvert.DeserializeObject<List<ModuleBase>>(row["modules"].ToString());

                        player.Equipment = row["configs"].ToString() != "" ? JsonConvert.DeserializeObject<EquipmentBase>(row["configs"].ToString()) : new EquipmentBase(player.Ship.BaseHitpoints, 0, 0, 300, player.Ship.BaseHitpoints, 0, 0, 300);
                    }
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
                    var deflectorTime = !battleStation.DeflectorActive ? $"deflectorTime = '{battleStation.deflectorTime.ToString("yyyy-MM-dd HH:mm:ss")}'," : "";

                    mySqlClient.ExecuteNonQuery($"UPDATE server_battlestations SET clanId = {battleStation.Clan.Id}," +
                    $"inBuildingState = {battleStation.InBuildingState}, buildTimeInMinutes = {battleStation.BuildTimeInMinutes}, {buildTime}" +
                    $"deflectorActive = {battleStation.DeflectorActive}, deflectorSecondsLeft = {battleStation.DeflectorSecondsLeft}, {deflectorTime} visualModifiers = '{JsonConvert.SerializeObject(visualModifiers)}' WHERE name = '{battleStation.AsteroidName}'");
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
                    int positionX = Convert.ToInt32(row["positionX"]);
                    int positionY = Convert.ToInt32(row["positionY"]);
                    var modules = JsonConvert.DeserializeObject<List<EquippedModuleBase>>(row["modules"].ToString());
                    var inBuildingState = Convert.ToBoolean(Convert.ToInt32(row["inBuildingState"]));
                    var buildTimeInMinutes = Convert.ToInt32(row["buildTimeInMinutes"]);
                    var buildTime = DateTime.Parse(row["buildTime"].ToString());
                    var deflectorActive = Convert.ToBoolean(Convert.ToInt32(row["deflectorActive"]));
                    var deflectorSecondsLeft = Convert.ToInt32(row["deflectorSecondsLeft"]);
                    var deflectorTime = DateTime.Parse(row["deflectorTime"].ToString());
                    var visualModifiers = JsonConvert.DeserializeObject<List<int>>(row["visualModifiers"].ToString());

                    var battleStation = new BattleStation(name, GameManager.GetSpacemap(mapId), new Position(positionX, positionY), GameManager.GetClan(clanId), modules, inBuildingState, buildTimeInMinutes, buildTime, deflectorActive, deflectorSecondsLeft, deflectorTime, visualModifiers);
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
                var data = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM chat_permissions WHERE userId = {userId}");
                foreach (DataRow row in data.Rows)
                {
                    return Convert.ToInt32(row["type"]);
                }
                return 0;
            }
        }
    }
}
