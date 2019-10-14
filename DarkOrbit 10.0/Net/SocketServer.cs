using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ow.Chat;
using Ow.Game;
using Ow.Game.Movements;
using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net.netty.commands;
using Ow.Utils;
using static Ow.Game.GameSession;

namespace Ow.Net
{
    class SocketServer
    {
        private static ManualResetEvent AllDone = new ManualResetEvent(false);
        public static int Port = 4301;

        public static void StartListening()
        {
            var localEndPoint = new IPEndPoint(IPAddress.Any, Port);
            var listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(-1);

                while (true)
                {
                    AllDone.Reset();
                    listener.BeginAccept(AcceptCallback, listener);
                    AllDone.WaitOne();
                }

            }
            catch (Exception)
            {
                Out.WriteLine("An application is already listening the port " + Port + ".", "ERROR", ConsoleColor.Red);
            }
        }

        private static readonly byte[] buffer = new byte[1024 * 3];

        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                AllDone.Set();
                var listener = (Socket)ar.AsyncState;
                var socket = listener.EndAccept(ar);
                socket.NoDelay = true;
                int count = socket.Receive(buffer);
                var json = Parse(Encoding.UTF8.GetString(buffer, 0, count));

                var parameters = Parse(json["Parameters"]);

                switch (String(json["Action"]))
                {
                    case "OnlineIds":
                        socket.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(GameManager.GameSessions.Keys).ToString()));
                        break;
                    case "OnlineCount":
                        socket.Send(Encoding.UTF8.GetBytes(GameManager.GameSessions.Count.ToString()));
                        break;
                    case "IsOnline":
                        var player = GameManager.GetPlayerById(Int(parameters["UserId"]));
                        var online = player != null ? true : false;
                        socket.Send(Encoding.UTF8.GetBytes(online.ToString()));
                        break;
                    case "IsInEquipZone":
                        player = GameManager.GetPlayerById(Int(parameters["UserId"]));
                        var inEquipZone = player != null ? player.Storage.IsInEquipZone : false;
                        socket.Send(Encoding.UTF8.GetBytes(inEquipZone.ToString()));
                        break;
                    case "BanUser":
                        BanUser(GameManager.GetPlayerById(Int(parameters["UserId"])));
                        break;
                    case "BuyItem":
                        BuyItem(GameManager.GetPlayerById(Int(parameters["UserId"])), String(parameters["ItemType"]), (DataType)Short(parameters["DataType"]), Int(parameters["Amount"]));
                        break;
                    case "ChangeClanData":
                        ChangeClanData(GameManager.GetClan(Int(parameters["ClanId"])), String(parameters["Name"]), String(parameters["Tag"]), Int(parameters["FactionId"]));
                        break;
                    case "ChangeShip":
                        ChangeShip(GameManager.GetPlayerById(Int(parameters["UserId"])), GameManager.GetShip(Int(parameters["ShipId"])));
                        break;
                    case "ChangeCompany":
                        ChangeCompany(GameManager.GetPlayerById(Int(parameters["UserId"])), Int(parameters["UridiumPrice"]), Int(parameters["HonorPrice"]));
                        break;
                    case "UpdateStatus":
                        UpdateStatus(GameManager.GetPlayerById(Int(parameters["UserId"])), Parse(parameters["Status"]));
                        break;
                    case "JoinToClan":
                        JoinToClan(GameManager.GetPlayerById(Int(parameters["UserId"])), GameManager.GetClan(Int(parameters["ClanId"])));
                        break;
                    case "LeaveFromClan":
                        LeaveFromClan(GameManager.GetPlayerById(Int(parameters["UserId"])));
                        break;
                    case "CreateClan":
                        CreateClan(GameManager.GetPlayerById(Int(parameters["UserId"])), Int(parameters["ClanId"]), Int(parameters["FactionId"]), String(parameters["Name"]), String(parameters["Tag"]));
                        break;
                    case "DeleteClan":
                        DeleteClan(GameManager.GetClan(Int(parameters["ClanId"])));
                        break;
                    case "StartDiplomacy":
                        StartDiplomacy(GameManager.GetClan(Int(parameters["SenderClanId"])), GameManager.GetClan(Int(parameters["TargetClanId"])), Short(parameters["DiplomacyType"]));
                        break;
                    case "EndDiplomacy":
                        EndDiplomacy(GameManager.GetClan(Int(parameters["SenderClanId"])), GameManager.GetClan(Int(parameters["TargetClanId"])));
                        break;
                    case "SkillTree":
                        UpgradeSkillTree(GameManager.GetPlayerById(Int(parameters["UserId"])), String(parameters["SkillName"]));
                        break;
                }
            }
            catch (Exception) { }
        }

        public static void UpgradeSkillTree(Player player, string skillName)
        {
            if (skillName == "skill_13")
                player.SkillTree.Engineering++;
            else if (skillName == "skill_5a")
                player.SkillTree.Detonation1++;
            else if (skillName == "skill_5b")
                player.SkillTree.Detonation2++;
            else if (skillName == "skill_20")
                player.SkillTree.HeatseekingMissiles++;
            else if (skillName == "skill_6")
                player.SkillTree.RocketFusion++;
            else if (skillName == "skill_21a")
                player.SkillTree.Cruelty1++;
            else if (skillName == "skill_21b")
                player.SkillTree.Cruelty2++;
            else if (skillName == "skill_1")
                player.SkillTree.Explosives++;
        }

        public static void BanUser(Player player)
        {
            if (player == null) return;

            var client = GameManager.ChatClients[player.Id];
            client.Send($"{ChatConstants.CMD_BANN_USER}%#");
            client.ShutdownConnection();

            player.GameSession.Disconnect(DisconnectionType.NORMAL);
            GameManager.SendChatSystemMessage($"{player.Name} has banned.");
        }

        public static void BuyItem(Player player, string itemType, DataType dataType, int amount)
        {
            if (player?.GameSession != null)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var result = mySqlClient.ExecuteQueryRow($"SELECT data FROM player_accounts WHERE userId = {player.Id}");
                    player.Data = JsonConvert.DeserializeObject<DataBase>(result["data"].ToString());
                }

                player.SendPacket($"0|LM|ST|{(dataType == DataType.URIDIUM ? "URI" : "CRE")}|-{amount}|{(dataType == DataType.URIDIUM ? player.Data.uridium : player.Data.credits)}");

                switch (itemType)
                {
                    case "drone":
                        player.DroneManager.UpdateDrones(true);
                        break;
                    case "booster":
                        var oldBoosters = player.BoosterManager.Boosters;

                        using (var mySqlClient = SqlDatabaseManager.GetClient())
                        {
                            var result = mySqlClient.ExecuteQueryRow($"SELECT boosters FROM player_equipment WHERE userId = {player.Id}");
                            var newBoosters = JsonConvert.DeserializeObject<Dictionary<short, List<BoosterBase>>>(result["boosters"].ToString());
                            player.BoosterManager.Boosters = newBoosters.Concat(oldBoosters).GroupBy(b => b.Key).ToDictionary(b => b.Key, b => b.First().Value);
                        }

                        player.BoosterManager.Update();
                        break;
                }
            }
        }

        public static void ChangeClanData(Clan clan, string name, string tag, int factionId)
        {
            if (clan.Id != 0)
            {
                clan.Tag = tag;
                clan.Name = name;
                //clan.FactionId = factionId;

                foreach (GameSession gameSession in GameManager.GameSessions.Values.Where(x => x.Player.Clan.Id == clan.Id))
                {
                    var player = gameSession.Player;
                    if (player != null)
                        GameManager.SendCommandToMap(player.Spacemap.Id, ClanChangedCommand.write(clan.Tag, clan.Id, player.Id));
                }
            }
        }

        public static void JoinToClan(Player player, Clan clan)
        {
            if (player?.GameSession != null && clan != null)
            {
                player.Clan = clan;

                var command = ClanChangedCommand.write(clan.Tag, clan.Id, player.Id);
                player.SendCommand(command);
                player.SendCommandToInRangePlayers(command);
            }
        }

        public static void EndDiplomacy(Clan senderClan, Clan targetClan)
        {
            if (senderClan != null && targetClan != null)
            {
                senderClan.Diplomacies.Remove(targetClan.Id);
                targetClan.Diplomacies.Remove(senderClan.Id);
            }
        }

        public static void StartDiplomacy(Clan senderClan, Clan targetClan, short diplomacyType)
        {
            if (senderClan != null && targetClan != null && new int[] {1,2,3}.Contains(diplomacyType))
            {
                senderClan.Diplomacies.Add(targetClan.Id, (Diplomacy)diplomacyType);
                targetClan.Diplomacies.Add(senderClan.Id, (Diplomacy)diplomacyType);
            }
        }

        public static void LeaveFromClan(Player player)
        {
            foreach (var battleStation in GameManager.BattleStations.Values)
            {
                if (battleStation.EquippedStationModule.ContainsKey(player.Clan.Id))
                    battleStation.EquippedStationModule[player.Clan.Id].ForEach(x => { if (x.OwnerId == player.Id) { x.Destroy(null, DestructionType.MISC); } });
            }

            if (player?.GameSession != null)
            {
                if (player.Clan.Id != 0)
                {
                    player.Clan = GameManager.GetClan(0);

                    var command = ClanChangedCommand.write(player.Clan.Tag, player.Clan.Id, player.Id);
                    player.SendCommand(command);
                    player.SendCommandToInRangePlayers(command);
                }
            }
        }

        public static void DeleteClan(Clan deletedClan)
        {
            if (deletedClan != null)
            {
                foreach (var battleStation in GameManager.BattleStations.Values.Where(x => x.Clan.Id == deletedClan.Id))
                    battleStation.Destroy(null, DestructionType.MISC);

                GameManager.Clans.TryRemove(deletedClan.Id, out deletedClan);

                foreach (var gameSession in GameManager.GameSessions.Values)
                {
                    var member = gameSession?.Player;

                    if (member != null && member.Clan.Id == deletedClan.Id)
                    {
                        member.Clan = GameManager.GetClan(0);

                        var command = ClanChangedCommand.write(member.Clan.Tag, member.Clan.Id, member.Id);
                        member.SendCommand(command);
                        member.SendCommandToInRangePlayers(command);
                    }
                }

                foreach (var clan in GameManager.Clans.Values)
                    clan.Diplomacies.Remove(deletedClan.Id);
            }
        }

        public static void CreateClan(Player player, int clanId, int factionId, string name, string tag)
        {
            if (!GameManager.Clans.ContainsKey(clanId))
            {
                var clan = new Clan(clanId, name, tag, factionId);
                GameManager.Clans.TryAdd(clan.Id, clan);

                if (player?.GameSession != null)
                {
                    player.Clan = clan;

                    var command = ClanChangedCommand.write(clan.Tag, clan.Id, player.Id);
                    player.SendCommand(command);
                    player.SendCommandToInRangePlayers(command);
                }
            }
        }

        public static void ChangeCompany(Player player, int uridiumPrice, int honorPrice)
        {
            if (player?.GameSession != null)
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                {
                    var result = mySqlClient.ExecuteQueryRow($"SELECT data, factionId FROM player_accounts WHERE userId = {player.Id}");
                    player.Data = JsonConvert.DeserializeObject<DataBase>(result["data"].ToString());
                    player.FactionId = Convert.ToInt32(result["factionId"]);
                }

                player.SendPacket($"0|LM|ST|URI|-{uridiumPrice}|{player.Data.uridium}");

                if (honorPrice > 0)
                    player.SendPacket($"0|LM|ST|HON|-{honorPrice}|{player.Data.honor}");

                player.Jump(player.GetBaseMapId(), player.GetBasePosition());
            }
        }

        public static void ChangeShip(Player player, Ship ship)
        {
            if (player?.GameSession != null && ship != null)
            {
                player.ChangeShip(ship.Id);
            }
        }

        public static void UpdateStatus(Player player, JObject status)
        {
            if (player?.GameSession != null)
            {
                player.Equipment.Config1Hitpoints = Int(status["Config1Hitpoints"]);
                player.Equipment.Config1Damage = Int(status["Config1Damage"]);
                player.Equipment.Config1Shield = Int(status["Config1Shield"]);
                player.Equipment.Config1Speed = Int(status["Config1Speed"]);
                player.Equipment.Config2Hitpoints = Int(status["Config2Hitpoints"]);
                player.Equipment.Config2Damage = Int(status["Config2Damage"]);
                player.Equipment.Config2Shield = Int(status["Config2Shield"]);
                player.Equipment.Config2Speed = Int(status["Config2Speed"]);

                player.DroneManager.UpdateDrones(true);
                player.UpdateStatus();
            }
        }

        public static int Int(object value)
        {
            return Convert.ToInt32(value.ToString());
        }

        public static short Short(object value)
        {
            return Convert.ToInt16(value.ToString());
        }

        public static string String(object value)
        {
            return value.ToString();
        }

        public static JObject Parse(object value)
        {
            return JObject.Parse(value.ToString());
        }
    }
}