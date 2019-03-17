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
using Ow.Game;
using Ow.Game.Clans;
using Ow.Game.Movements;
using Ow.Game.Objects;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Utils;

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
                listener.Listen(100);

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
                            CreateClan(GameManager.GetPlayerById(Int(parameters["UserId"])), Int(parameters["ClanId"]), parameters["Name"], parameters["Tag"]);
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

        public static void JoinToClan(Player player, Clan clan)
        {
            if (player.GameSession == null || player == null || clan == null) return;

            player.Clan = clan;
            GameManager.SendCommandToMap(player.Spacemap.Id, ClanChangedCommand.write(clan.Tag, clan.Id, player.Id));
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
            if (senderClan != null && targetClan != null && (diplomacyType == 1 || diplomacyType == 2 || diplomacyType == 3))
            {
                senderClan.Diplomacies.Add(targetClan.Id, (Diplomacy)diplomacyType);
                targetClan.Diplomacies.Add(senderClan.Id, (Diplomacy)diplomacyType);
            }
        }

        public static void LeaveFromClan(Player player)
        {
            if (player.GameSession == null || player == null) return;

            if (player.Clan != null && player.Clan.Id != 0)
            {
                player.Clan = GameManager.GetClan(0);
                GameManager.SendCommandToMap(player.Spacemap.Id, ClanChangedCommand.write("", 0, player.Id));
            }
        }

        public static void DeleteClan(Clan deletedClan)
        {
            if (deletedClan != null)
            {
                GameManager.Clans.TryRemove(deletedClan.Id, out deletedClan);

                foreach (var gameSession in GameManager.GameSessions.Values)
                {
                    var member = gameSession.Player;

                    if (member.Clan == deletedClan)
                    {
                        member.Clan = GameManager.GetClan(0);
                        GameManager.SendCommandToMap(member.Spacemap.Id, ClanChangedCommand.write("", 0, member.Id));
                    }
                }

                foreach (var clan in GameManager.Clans.Values)
                    clan.Diplomacies.Remove(deletedClan.Id);
            }
        }

        public static void CreateClan(Player player, int clanId, object name, object tag)
        {
            var clan = new Clan(clanId, name.ToString(), tag.ToString(), 0);
            GameManager.Clans.TryAdd(clan.Id, clan);

            if (player.GameSession == null || player == null) return;

            player.Clan = clan;
            GameManager.SendCommandToMap(player.Spacemap.Id, ClanChangedCommand.write(clan.Tag, clan.Id, player.Id));
        }

        public static void ChangeCompany(Player player, int factionId, int uridiumPrice, int honorPrice)
        {
            if (player.GameSession == null || player == null) return;

            if (player.Settings.InGameSettings.inEquipZone && player.FactionId != factionId && (factionId == 1 || factionId == 2 || factionId == 3))
            {
                player.ChangeData(DataType.URIDIUM, uridiumPrice, ChangeType.DECREASE);
                player.ChangeData(DataType.HONOR, honorPrice, ChangeType.DECREASE);

                player.FactionId = factionId;
                player.Jump(player.GetBaseMapId(), factionId == 1 ? Position.MMOPosition : factionId == 2 ? Position.EICPosition : Position.VRUPosition);
            }
        }

        public static void ChangeShip(Player player, int shipId)
        {
            if (player.GameSession == null || player == null) return;

            if (GameManager.Ships.ContainsKey(shipId))
                player.ChangeShip(shipId);
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

            player.DroneManager.UpdateDrones();
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