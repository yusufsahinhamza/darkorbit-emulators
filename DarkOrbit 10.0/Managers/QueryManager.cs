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
                    mySqlClient.ExecuteNonQuery($"UPDATE player_accounts SET data = '{JsonConvert.SerializeObject(player.Data)}', nanohull = {player.CurrentNanoHull}, destructions = '{JsonConvert.SerializeObject(player.Destructions)}'  WHERE userId = {player.Id}");
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
            public static void AddBan(int bannedId, int modId, string reason, int typeId, string endDate)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var result = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT userId FROM player_accounts WHERE userId = {bannedId}");
                    if (result.Rows.Count >= 1)
                    {
                        mySqlClient.ExecuteNonQuery($"INSERT INTO server_bans (userId, modId, reason, typeId, end_date) VALUES ({bannedId}, {modId}, '{reason}', {typeId}, '{endDate}')");

                        GameManager.SendChatSystemMessage($"{QueryManager.GetUserPilotName(bannedId)} has banned.");
                    }
                }
            }

            public static void UnBan(int bannedId, int modId, int typeId)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var result = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM server_bans WHERE userId = {bannedId} AND typeId = {typeId}");
                    if (result.Rows.Count >= 1)
                    {
                        mySqlClient.ExecuteNonQuery($"UPDATE server_bans SET ended = 1 WHERE userId = {bannedId} AND typeId = {typeId}");

                        var client = GameManager.ChatClients[modId];

                        if (client != null)
                            client.Send($"{QueryManager.GetUserPilotName(bannedId)} has unbanned.");
                    }
                }
            }

            public static bool Banned(int userId)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var result = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT id FROM server_bans WHERE userId = {userId} AND typeId = 0 AND ended = 0");
                    return result.Rows.Count >= 1 ? true : false;
                }
            }
        }

        public static string GetUserPilotName(int userId)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var result = mySqlClient.ExecuteQueryRow($"SELECT pilotName FROM player_accounts WHERE userId = {userId}");
                return result["pilotName"].ToString();
            }
        }

        public static bool CheckSessionId(int userId, string sessionId)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var query = $"SELECT sessionId FROM player_accounts WHERE userId = {userId}";
                var table = (DataTable)mySqlClient.ExecuteQueryTable(query);

                if (table.Rows.Count >= 1)
                {
                    var result = mySqlClient.ExecuteQueryRow(query);
                    return sessionId == result["sessionId"].ToString();
                }
                else return false;
            }
        }

        public static bool Banned(int userId)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var result = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT id FROM server_bans WHERE userId = {userId} AND typeId = 1 AND ended = 0");
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
                    var data = mySqlClient.ExecuteQueryTable($"SELECT * FROM player_accounts WHERE userId = {playerId}");
                    foreach (DataRow row in data.Rows)
                    {
                        var name = Convert.ToString(row["pilotName"]);
                        var ship = GameManager.GetShip(Convert.ToInt32(row["shipId"]));
                        var factionId = Convert.ToInt32(row["factionId"]);
                        var rankId = Convert.ToInt32(row["rankID"]);
                        var warRank = Convert.ToInt32(row["warRank"]);
                        var clan = GameManager.GetClan(Convert.ToInt32(row["clanID"]));

                        player = new Player(playerId, name, clan, factionId, rankId, warRank, ship);
                        player.Premium = Convert.ToBoolean(row["premium"]);
                        player.Title = Convert.ToString(row["title"]);
                        player.Data = JsonConvert.DeserializeObject<DataBase>(row["data"].ToString());
                        player.Destructions = JsonConvert.DeserializeObject<DestructionsBase>(row["destructions"].ToString());
                        player.CurrentNanoHull = Convert.ToInt32(row["nanohull"]);
                        player.PetName = Convert.ToString(row["petName"]);
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
                        player.SkillTree = JsonConvert.DeserializeObject<SkillTreeBase>(row["skill_points"].ToString());

                        dynamic items = JsonConvert.DeserializeObject(row["items"].ToString());

                        if (items["pet"] == "true")
                            player.Pet = new Pet(player);
                    }
                }

                SetEquipment(player);

                return player;
            }
            catch (Exception e)
            {
                Logger.Log("error_log", $"- [QueryManager.cs] GetPlayer({playerId}) exception: {e}");
                return null;
            }
        }

        public static void SetEquipment(Player player)
        {
            try
            {
                var lf3Damage = 150;
                var lf4Damage = 200;
                var bo2Shield = 15000;
                var g3nSpeed = 10;

                var hitpoints = new int[] { player.Ship.BaseHitpoints + 60000, player.Ship.BaseHitpoints + 60000 };
                var speed = new int[] { player.Ship.BaseSpeed, player.Ship.BaseSpeed };
                var damage = new int[] { 0, 0 };
                var shield = new int[] { 0, 0 };

                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var equipment = mySqlClient.ExecuteQueryTable($"SELECT * FROM player_equipment WHERE userId = {player.Id}");

                    foreach (DataRow row in equipment.Rows)
                    {
                        dynamic items = JsonConvert.DeserializeObject(row["items"].ToString());

                        for (var i = 1; i <= 2; i++)
                        {
                            foreach (int itemId in (dynamic)JsonConvert.DeserializeObject(row[$"config{i}_lasers"].ToString()))
                            {
                                if (itemId >= 0 && itemId < 40)
                                    damage[i - 1] += lf3Damage;
                                else if (itemId >= 140)
                                    damage[i - 1] += lf4Damage;
                            }

                            foreach (int itemId in (dynamic)JsonConvert.DeserializeObject(row[$"config{i}_generators"].ToString()))
                            {
                                if (itemId >= 40 && itemId < 100)
                                    shield[i - 1] += bo2Shield;
                                else if (itemId >= 100 && itemId < 120)
                                    speed[i - 1] += g3nSpeed;
                            }

                            var havocCount = 0;
                            var herculesCount = 0;

                            var drones = (dynamic)JsonConvert.DeserializeObject(row[$"config{i}_drones"].ToString());

                            foreach (var drone in drones)
                            {
                                var herculesEquipped = false;

                                foreach (int design in drone["designs"])
                                {
                                    if (design >= 120 && design < 130)
                                        havocCount++;
                                    else if (design >= 130 && design < 140)
                                    {
                                        herculesEquipped = true;
                                        herculesCount++;
                                    }
                                }

                                var droneShield = bo2Shield + 2000;

                                foreach (int item in drone["items"])
                                {
                                    if (item >= 0 && item < 40)
                                        damage[i - 1] += lf3Damage + 15;
                                    else if (item >= 140)
                                        damage[i - 1] += lf4Damage + 20;
                                    else if (item >= 40 && item < 100)
                                        shield[i - 1] += droneShield + (herculesEquipped ? +Maths.GetPercentage(droneShield, 15) : 0);
                                }
                            }

                            if (havocCount == drones.Count)
                                damage[i - 1] += Maths.GetPercentage(damage[i - 1], 10);
                            else if (herculesCount == 10)
                                hitpoints[i - 1] += Maths.GetPercentage(hitpoints[i - 1], 20);
                        }

                        speed[0] += Maths.GetPercentage(speed[0], 20);
                        speed[1] += Maths.GetPercentage(speed[1], 20);

                        var configsBase = new ConfigsBase(hitpoints[0], damage[0], shield[0], speed[0], hitpoints[1], damage[1], shield[1], speed[1]);
                        var itemsBase = new ItemsBase(0);//TODO = new ItemsBase((int)items["bootyKeys"]);

                        player.Equipment = new EquipmentBase(configsBase, itemsBase);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log("error_log", $"- [QueryManager.cs] SetEquipment({player.Id}) exception: {e}");
            }
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
                    var npcs = JsonConvert.DeserializeObject<List<NpcsBase>>(row["npcs"].ToString());
                    var portals = JsonConvert.DeserializeObject<List<PortalBase>>(row["portals"].ToString());
                    var stations = JsonConvert.DeserializeObject<List<StationBase>>(row["stations"].ToString());
                    var options = JsonConvert.DeserializeObject<OptionsBase>(row["options"].ToString());
                    var spacemap = new Spacemap(mapId, name, factionId, npcs, portals, stations, options);
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
                    bool active = Convert.ToBoolean(row["active"]);

                    if (active)
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
                    int damage = Convert.ToInt32(row["damage"]);
                    int shields = Convert.ToInt32(row["shield"]);
                    int hitpoints = Convert.ToInt32(row["health"]);
                    int speed = Convert.ToInt32(row["speed"]);
                    string lootID = Convert.ToString(row["lootID"]);
                    bool aggressive = Convert.ToBoolean(row["aggressive"]);
                    bool respawnable = Convert.ToBoolean(row["respawnable"]);
                    var rewards = JsonConvert.DeserializeObject<ShipRewards>(row["reward"].ToString());

                    var ship = new Ship(name, shipID, hitpoints, shields, speed, lootID, damage, aggressive, respawnable, rewards);
                    GameManager.Ships.TryAdd(ship.Id, ship);
                }
            }
        }

        public static void LoadClans()
        {
            GameManager.Clans.TryAdd(0, new Clan(0, "", "", 0));
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var data = (DataTable)mySqlClient.ExecuteQueryTable("SELECT * FROM server_clans");
                foreach (DataRow row in data.Rows)
                {
                    int id = Convert.ToInt32(row["id"]);
                    string name = Convert.ToString(row["name"]);
                    string tag = Convert.ToString(row["tag"]);
                    int factionId = Convert.ToInt32(row["factionId"]);

                    var clan = new Clan(id, name, tag, factionId);
                    GameManager.Clans.TryAdd(clan.Id, clan);
                    LoadClanDiplomacy(clan);
                }
            }
        }

        private static void LoadClanDiplomacy(Clan clan)
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                var data = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM server_clan_diplomacy WHERE senderClanId = {clan.Id}");
                foreach (DataRow row in data.Rows)
                {
                    int id = Convert.ToInt32(row["toClanId"]);
                    Diplomacy relation = (Diplomacy)Convert.ToInt32(row["diplomacyType"]);
                    clan.Diplomacies.Add(id, relation);
                }

                var data2 = (DataTable)mySqlClient.ExecuteQueryTable($"SELECT * FROM server_clan_diplomacy WHERE toClanId = {clan.Id}");
                foreach (DataRow row in data2.Rows)
                {
                    int id = Convert.ToInt32(row["senderClanId"]);
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
