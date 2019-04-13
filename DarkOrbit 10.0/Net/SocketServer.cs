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
                int count = socket.Receive(buffer);
                var json = Parse(Encoding.UTF8.GetString(buffer, 0, count));

                if (ValidateJSON(String(json)))
                {
                    var parameters = Parse(json["Parameters"]);
                    
                    switch (String(json["Action"]))
                    {
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
                            ChangeClanData(GameManager.GetClan(Int(parameters["ClanId"])), parameters["Name"], parameters["Tag"]);
                            break;
                        case "ChangeShip":
                            ChangeShip(GameManager.GetPlayerById(Int(parameters["UserId"])), Int(parameters["ShipId"]));
                            break;
                        case "ChangeCompany":
                            ChangeCompany(GameManager.GetPlayerById(Int(parameters["UserId"])), Int(parameters["FactionId"]), Int(parameters["UridiumPrice"]), Int(parameters["HonorPrice"]));
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
                            CreateClan(GameManager.GetPlayerById(Int(parameters["UserId"])), Int(parameters["ClanId"]), Int(parameters["FactionId"]), parameters["Name"], parameters["Tag"]);
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
                    }
                }
            }
            catch (Exception) { }
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
            if (player.GameSession == null || player == null) return;

            player.ChangeData(dataType, amount, ChangeType.DECREASE);

            switch (itemType)
            {
                case "drone":
                    player.DroneManager.UpdateDrones(true);
                    break;
            }
        }

        public static void ChangeClanData(Clan clan, object name, object tag)
        {
            if (clan.Id != 0)
            {
                clan.Tag = tag.ToString();
                clan.Name = name.ToString();
                foreach (GameSession gameSession in GameManager.GameSessions.Values)
                {
                    var player = gameSession.Player;
                    if (player.Clan == clan)
                        GameManager.SendCommandToMap(player.Spacemap.Id, ClanChangedCommand.write(clan.Tag, clan.Id, player.Id));
                }
            }
        }

        public static void JoinToClan(Player player, Clan clan)
        {
            if (player.GameSession == null || player == null || clan == null) return;

            player.Clan = clan;
            GameManager.SendCommandToMap(player.Spacemap.Id, ClanChangedCommand.write(clan.Tag, clan.Id, player.Id));
        }

        public static void EndDiplomacy(Clan senderClan, Clan targetClan)
        {
            if (senderClan.Id != 0 && targetClan.Id != 0)
            {
                senderClan.Diplomacies.Remove(targetClan.Id);
                targetClan.Diplomacies.Remove(senderClan.Id);
            }
        }

        public static void StartDiplomacy(Clan senderClan, Clan targetClan, short diplomacyType)
        {
            if (senderClan.Id != 0 && targetClan.Id != 0 && new int[] {1,2,3}.Contains(diplomacyType))
            {
                senderClan.Diplomacies.Add(targetClan.Id, (Diplomacy)diplomacyType);
                targetClan.Diplomacies.Add(senderClan.Id, (Diplomacy)diplomacyType);
            }
        }

        public static void LeaveFromClan(Player player)
        {
            if (player.GameSession == null || player == null) return;

            if (player.Clan.Id != 0)
            {
                player.Clan = GameManager.GetClan(0);
                GameManager.SendCommandToMap(player.Spacemap.Id, ClanChangedCommand.write(player.Clan.Tag, player.Clan.Id, player.Id));
            }
        }

        public static void DeleteClan(Clan deletedClan)
        {
            if (deletedClan.Id != 0)
            {
                GameManager.Clans.TryRemove(deletedClan.Id, out deletedClan);

                foreach (var gameSession in GameManager.GameSessions.Values)
                {
                    var member = gameSession.Player;

                    if (member.Clan == deletedClan)
                    {
                        member.Clan = GameManager.GetClan(0);
                        GameManager.SendCommandToMap(member.Spacemap.Id, ClanChangedCommand.write(member.Clan.Tag, member.Clan.Id, member.Id));
                    }
                }

                foreach (var clan in GameManager.Clans.Values)
                    clan.Diplomacies.Remove(deletedClan.Id);
            }
        }

        public static void CreateClan(Player player, int clanId, int factionId, object name, object tag)
        {
            var clan = new Clan(clanId, name.ToString(), tag.ToString(), factionId, 0);
            GameManager.Clans.TryAdd(clan.Id, clan);

            if (player.GameSession == null || player == null) return;

            player.Clan = clan;
            GameManager.SendCommandToMap(player.Spacemap.Id, ClanChangedCommand.write(clan.Tag, clan.Id, player.Id));
        }

        public static void ChangeCompany(Player player, int factionId, int uridiumPrice, int honorPrice)
        {
            if (player.GameSession == null || player == null) return;

            if (player.Storage.IsInEquipZone && player.FactionId != factionId && new int[] {1,2,3}.Contains(factionId))
            {
                using (var mySqlClient = SqlDatabaseManager.GetClient())
                    mySqlClient.ExecuteNonQuery($"UPDATE player_accounts SET factionID = {factionId} WHERE userID = {player.Id}");

                player.ChangeData(DataType.URIDIUM, uridiumPrice, ChangeType.DECREASE);
                player.ChangeData(DataType.HONOR, honorPrice, ChangeType.DECREASE);

                player.FactionId = factionId;
                player.Jump(player.GetBaseMapId(), player.GetBasePosition());
            }
        }

        public static void ChangeShip(Player player, int shipId)
        {
            if (player.GameSession == null || player == null) return;

            if (GameManager.Ships.ContainsKey(shipId))
                player.ChangeShip(shipId);
        }

        public static void UpdateData()
        {
        
        }

        public static void UpdateStatus(Player player, JObject status)
        {
            if (player.GameSession == null || player == null) return;

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

        public static bool ValidateJSON(string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch
            {
                return false;
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