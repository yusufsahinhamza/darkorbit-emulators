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
        private static int Port = 4301;

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
                    var player = GameManager.GetPlayerById(Int(Parse(json["Parameters"])["UserId"]));
                    if (player.GameSession != null && player != null)
                    {
                        switch (String(json["Action"]))
                        {
                            case "ChangeShip":
                                ChangeShip(player, Int(Parse(json["Parameters"])["ShipId"]));
                                break;
                            case "ChangeCompany":
                                ChangeCompany(player, Int(Parse(json["Parameters"])["FactionId"]));
                                break;
                            case "UpdateStatus":
                                UpdateStatus(player, Parse(Parse(json["Parameters"])["Status"]));
                                break;
                            case "CreateClan":
                                CreateClan(player, Int(Parse(json["Parameters"])["ClanId"]), Parse(json["Parameters"])["Name"], Parse(json["Parameters"])["Tag"]);
                                break;
                            case "DeleteClan":
                                DeleteClan(player, Int(Parse(json["Parameters"])["ClanId"]));
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //ignore
            }
        }

        public static void DeleteClan(Player player, int clanId)
        {
            var deletedClan = GameManager.Clans[clanId];

            if (deletedClan != null)
            {
                GameManager.Clans.TryRemove(deletedClan.Id, out deletedClan);

                foreach (var gameSession in GameManager.GameSessions.Values)
                {
                    var member = gameSession.Player;

                    if (member.Clan == deletedClan)
                    {
                        member.Clan = null;
                        GameManager.SendCommandToMap(member.Spacemap.Id, ClanChangedCommand.write("", 0, member.Id));
                        member.SendPacket($"0|A|STD|Leader {player.Name} has deleted this clan!");
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
            GameManager.SendCommandToMap(player.Spacemap.Id, ClanChangedCommand.write(clan.Tag, clan.Id, player.Id));
        }

        public static void ChangeCompany(Player player, int factionId)
        {
            if (player.Settings.InGameSettings.inEquipZone && !player.AttackingOrUnderAttack() && player.FactionId != factionId && (factionId == 1 || factionId == 2 || factionId == 3))
            {
                player.FactionId = factionId;
                player.Jump(factionId == 1 ? 13 : factionId == 2 ? 14 : 15, factionId == 1 ? Position.MMOPosition : factionId == 2 ? Position.EICPosition : Position.VRUPosition);
            }
        }

        public static void ChangeShip(Player player, int shipId)
        {
            if (GameManager.Ships.ContainsKey(shipId))
                player.ChangeShip(shipId);
        }

        public static void UpdateStatus(Player player, JObject status)
        {
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