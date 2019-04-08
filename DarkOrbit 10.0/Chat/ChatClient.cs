using Ow.Game.Events;
using Ow.Game.Objects;
using Ow.Game.Movements;
using Ow.Managers;
using Ow.Net;
using Ow.Net.netty;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Ow.Game;
using static Ow.Game.GameSession;
using System.Collections.Concurrent;

namespace Ow.Chat
{
    public enum Permissions
    {
        NORMAL = 0,
        ADMINISTRATOR = 1,
        CHAT_MODERATOR = 2
    }

    class ChatClient
    {
        public Socket Socket { get; set; }
        public bool SocketClosed { get; set; }
        private readonly byte[] buffer = new byte[2048];

        public int UserId { get; set; }
        public Permissions Permission { get; set; }
        public List<Int32> ChatsJoined = new List<Int32>();

        public static List<string> Filter = new List<string>
        {
            "orospu",
            "çocuğu",
            "karını",
            "sülaleni",
            "dinini",
            "imanını",
            "kitabını",
            "siker",
            "lavuk",
            "ananı",
            "gavat",
            "oneultimate",
            "http",
            "bitch",
            "fuck",
            "lag",
            "restart",
            "amına",
            "piç",
            "yavşak",
            "siktir",
            "anne",
            "bacı",
            "sikerim",
            "pezevenk",
            ".ovh",
            "puto",
            "maldito",
            "perro"
        };

        public ChatClient(Socket Socket)
        {
            this.Socket = Socket;
            try
            {
                if (!Socket.IsBound && !Socket.Connected) new Exception("Unable to read. Socket is not bound or connected.");

                this.Socket.BeginReceive(buffer, 0, buffer.Length, 0, ReadCallback, this);
            }
            catch (Exception e)
            {
                Out.WriteLine("Error: " + e.Message, "", ConsoleColor.Red);
            }
        }

        public void Execute(string message)
        {
            try
            {
                string[] packet = message.Split(ChatConstants.MSG_SEPERATOR);
                switch (packet[0])
                {
                    case ChatConstants.CMD_USER_LOGIN:
                        var loginPacket = message.Replace("@", "%").Split('%');
                        UserId = Convert.ToInt32(loginPacket[3]);

                        var gameSession = GameManager.GetGameSession(UserId);
                        if (gameSession == null) return;

                        Permission = (Permissions)QueryManager.GetChatPermission(gameSession.Player.Id);

                        if (GameManager.ChatClients.ContainsKey(UserId))
                            GameManager.ChatClients[gameSession.Player.Id]?.ShutdownConnection();

                        GameManager.ChatClients.TryAdd(gameSession.Player.Id, this);

                        Send("bv%" + gameSession.Player.Id + "#");
                        var servers = Room.Rooms.Aggregate(String.Empty, (current, chat) => current + chat.Value.ToString());
                        servers = servers.Remove(servers.Length - 1);
                        Send("by%" + servers + "#");
                        Send($"dq%Use '/duel name' for invite someone to duel.#");
                        ChatsJoined.Add(Room.Rooms.FirstOrDefault().Value.Id);

                        if (QueryManager.ChatFunctions.Banned(UserId))
                        {
                            Send($"{ChatConstants.CMD_BANN_USER}%#");
                            ShutdownConnection();
                            return;
                        }
                        break;
                    case ChatConstants.CMD_USER_MSG:
                        SendMessage(message);
                        break;
                    case ChatConstants.CMD_USER_JOIN:
                        var roomId = Convert.ToInt32(message.Split('%')[2].Split('@')[0]);
                        gameSession = GameManager.GetGameSession(UserId);

                        if (Room.Rooms.ContainsKey(roomId))
                        {
                            if (!ChatsJoined.Contains(roomId))
                                ChatsJoined.Add(roomId);
                        }
                        else
                        {
                            if (gameSession.Player.Storage.DuelInvites.ContainsKey(roomId))
                                AcceptDuel(gameSession.Player.Storage.DuelInvites[roomId], roomId);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("Exception: " + e, "ChatClient.cs");
            }
        }

        public void AcceptDuel(Player inviterPlayer, int duelId)
        {          
            var gameSession = GameManager.GetGameSession(UserId);

            if (!gameSession.Player.Storage.DuelInvites.ContainsKey(duelId))
            {
                Send($"dq%This invite is no longer available.#");
                return;
            }

            if (gameSession.Player.Storage.DuelOpponent != null)
            {
                Send($"dq%You cant accept duels while your duel continuenigenbeninn.#");
                return;
            }

            if (inviterPlayer.Storage.DuelOpponent != null)
            {
                Send($"dq%Your opponent is already fighting on duel with another player.#");
                return;
            }

            if (inviterPlayer != null && gameSession.Player != null)
            {
                if (gameSession.Player.Settings.InGameSettings.inEquipZone && inviterPlayer.Settings.InGameSettings.inEquipZone)
                {
                    gameSession.Player.Storage.DuelInvites.TryRemove(duelId, out inviterPlayer);
                    var players = new ConcurrentDictionary<int, Player>();
                    players.TryAdd(gameSession.Player.Id, gameSession.Player);
                    players.TryAdd(inviterPlayer.Id, inviterPlayer);

                    new Duel(players);
                }
            }          
        }

        public void SendMessage(string content)
        {
            var gameSession = GameManager.GetGameSession(UserId);
            if (gameSession == null) return;

            gameSession.LastActiveTime = DateTime.Now;
            string messagePacket = "";

            var packet = content.Replace("@", "%").Split('%');
            var roomId = packet[1];
            var message = packet[2];

            var cmd = message.Split(' ')[0];
            if (message.StartsWith("/reconnect"))
            {
                ShutdownConnection();
            }
            else if (cmd == "/w")
            {
                if (message.Split(' ').Length < 2)
                {
                    Send($"{ChatConstants.CMD_NO_WHISPER_MESSAGE}%#");
                    return;
                }

                var userName = message.Split(' ')[1];
                var player = GameManager.GetPlayerByName(userName);

                if (player == null || !GameManager.ChatClients.ContainsKey(player.Id))
                {
                    Send($"{ChatConstants.CMD_USER_NOT_EXIST}%#");
                    return;
                }

                if (player.Name == gameSession.Player.Name)
                {
                    Send($"dq%You can't whisper to yourself.#");
                    //Send($"{ChatConstants.CMD_CANNOT_WHISPER_YOURSELF}%#");
                    return;
                }

                message = message.Remove(0, player.Name.Length + 3);
                GameManager.ChatClients[player.Id].Send("cv%" + gameSession.Player.Name + "@" + message + "#");
                Send("cw%" + player.Name + "@" + message + "#");
            }
            else if (cmd == "/kick" && (Permission == Permissions.ADMINISTRATOR || Permission == Permissions.CHAT_MODERATOR))
            {
                if (message.Split(' ').Length < 2) return;

                var userId = Convert.ToInt32(message.Split(' ')[1]);
                var player = GameManager.GetPlayerById(userId);

                if (player != null && player.Name != gameSession.Player.Name)
                {
                    if (GameManager.ChatClients.ContainsKey(player.Id))
                    {
                        var client = GameManager.ChatClients[player.Id];
                        client.Send($"{ChatConstants.CMD_KICK_USER}%#");
                        client.ShutdownConnection();

                        GameManager.SendChatSystemMessage($"{player.Name} has kicked.");
                    }
                }
            }
            else if (cmd == "/duel")
            {
                if (message.Split(' ').Length < 2)
                {
                    Send($"dq%Use '/duel name' for invite someone to duel.#");
                    return;
                }

                var userName = message.Split(' ')[1];
                var duelPlayer = GameManager.GetPlayerByName(userName);

                if (duelPlayer == null || !GameManager.ChatClients.ContainsKey(duelPlayer.Id))
                {
                    Send($"{ChatConstants.CMD_USER_NOT_EXIST}%#");
                    return;
                }

                if (duelPlayer.Name == gameSession.Player.Name)
                {
                    Send($"{ChatConstants.CMD_CANNOT_INVITE_YOURSELF}%#");
                    return;
                }

                if (duelPlayer == null || duelPlayer == gameSession.Player || !GameManager.ChatClients.ContainsKey(duelPlayer.Id)) return;
                if (duelPlayer.Storage.DuelInvites.Any(x => x.Value == gameSession.Player))
                {
                    Send($"dq%{userName} already invited from you before.#");
                    return;
                }

                var duelId = Randoms.CreateRandomID();

                Send($"cr%{duelPlayer.Name}#");
                duelPlayer.Storage.DuelInvites.TryAdd(duelId, gameSession.Player);

                GameManager.ChatClients[duelPlayer.Id].Send("cj%" + duelId + "@" + "Duel" + "@" + 0 + "@" + gameSession.Player.Name + "#");
            }
            else if (cmd == "/msg" && Permission == Permissions.ADMINISTRATOR)
            {
                var msg = message.Remove(0, 4);
                GameManager.SendPacketToAll($"0|A|STD|{msg}");
            }
            else if (cmd == "/destroy" && Permission == Permissions.ADMINISTRATOR)
            {
                if (message.Split(' ').Length < 2) return;

                var userId = Convert.ToInt32(message.Split(' ')[1]);
                var player = GameManager.GetPlayerById(userId);

                if (player == null || !GameManager.ChatClients.ContainsKey(player.Id))
                {
                    Send($"{ChatConstants.CMD_USER_NOT_EXIST}%#");
                    return;
                }

                player.Destroy(gameSession.Player, Game.DestructionType.PLAYER);
            }
            else if (cmd == "/ship" && Permission == Permissions.ADMINISTRATOR)
            {
                if (message.Split(' ').Length < 2) return;

                var shipId = Convert.ToInt32(message.Split(' ')[1]);

                gameSession.Player.Ship = GameManager.GetShip(shipId);
                gameSession.Player.Jump(gameSession.Player.Spacemap.Id, gameSession.Player.Position);
            }
            else if (cmd == "/jump" && Permission == Permissions.ADMINISTRATOR)
            {
                if (message.Split(' ').Length < 2) return;

                var mapId = Convert.ToInt32(message.Split(' ')[1]);
                gameSession.Player.Jump(mapId, new Position(0, 0));
            }
            else if (cmd == "/move" && Permission == Permissions.ADMINISTRATOR)
            {
                if (message.Split(' ').Length < 3) return;

                var userId = Convert.ToInt32(message.Split(' ')[1]);
                var mapId = Convert.ToInt32(message.Split(' ')[2]);
                GameManager.GetPlayerById(userId)?.Jump(mapId, new Position(0, 0));
            }
            else if (cmd == "/speed+" && Permission == Permissions.ADMINISTRATOR)
            {
                if (message.Split(' ').Length < 2) return;

                var speed = Convert.ToInt32(message.Split(' ')[1]);
                gameSession.Player.SetSpeedBoost(speed);
            }
            else if (cmd == "/god" && Permission == Permissions.ADMINISTRATOR)
            {
                if (message.Split(' ').Length < 2) return;

                var mod = message.Split(' ')[1];
                gameSession.Player.Storage.GodMode = mod == "on" ? true : mod == "off" ? false : false;
            }

            else if (cmd == "/start_spaceball" && Permission == Permissions.ADMINISTRATOR)
            {
                EventManager.Spaceball.Start();
            }
            else if (cmd == "/stop_spaceball" && Permission == Permissions.ADMINISTRATOR)
            {
                EventManager.Spaceball.Stop();
            }
            else if (cmd == "/start_jpb" && Permission == Permissions.ADMINISTRATOR)
            {
                EventManager.JackpotBattle.Start();
            }
            else if (cmd == "/ban" && (Permission == Permissions.ADMINISTRATOR || Permission == Permissions.CHAT_MODERATOR))
            {
                /*
                0 CHAT BAN
                1 OYUN BANI
                */
                if (message.Split(' ').Length < 4) return;

                var userId = Convert.ToInt32(message.Split(' ')[1]);
                var typeId = Convert.ToInt32(message.Split(' ')[2]);
                var day = Convert.ToInt32(message.Split(' ')[3]);
                var reason = message.Remove(0, (userId.ToString().Length + typeId.ToString().Length + day.ToString().Length) + 5);

                if (typeId == 1 && Permission == Permissions.CHAT_MODERATOR) return;

                if (typeId == 0 || typeId == 1)
                {
                    var player = GameManager.GetPlayerById(userId);
                    if (player == null) return;               

                    var client = GameManager.ChatClients[player.Id];
                    client.Send($"{ChatConstants.CMD_BANN_USER}%#");
                    client.ShutdownConnection();

                    if (typeId == 1)
                        player.GameSession.Disconnect(DisconnectionType.NORMAL);

                    QueryManager.ChatFunctions.AddBan(player.Id, gameSession.Player.Id, reason, typeId, (DateTime.Now.AddDays(day)).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    GameManager.SendChatSystemMessage($"{player.Name} has banned.");
                }
            }
            else if (cmd == "/restart" && Permission == Permissions.ADMINISTRATOR)
            {
                if (message.Split(' ').Length < 2) return;

                var seconds = Convert.ToInt32(message.Split(' ')[1]);
                GameManager.Restart(seconds);
            }
            else if (cmd == "/users")
            {
                var users = GameManager.GameSessions.Values.Aggregate(String.Empty, (current, user) => current + user.Player.Name + ", ");
                users = users.Remove(users.Length - 2);

                Send($"dq%Users online {GameManager.GameSessions.Count}: {users}#");
            }
            else if (cmd == "/system")
            {
                message = message.Remove(0, 8);
                GameManager.SendChatSystemMessage(message);
            }
            else if (cmd == "/id")
            {
                if (message.Split(' ').Length < 2) return;
                var player = GameManager.GetPlayerByName(message.Split(' ')[1]);

                if (player == null || !GameManager.ChatClients.ContainsKey(player.Id))
                {
                    Send($"{ChatConstants.CMD_USER_NOT_EXIST}%#");
                    return;
                }

                Send($"dq%{player.Name} id : {player.Id}#");
            }
            else if (cmd == "/reward" && Permission == Permissions.ADMINISTRATOR)
            {
                if (message.Split(' ').Length < 4) return;
                var userId = Convert.ToInt32(message.Split(' ')[1]);
                var typeId = Convert.ToInt32(message.Split(' ')[2]); //1 uridium / 2 credits / 3 honor / 4 experience
                var amount = Convert.ToInt32(message.Split(' ')[3]);

                var player = GameManager.GetPlayerById(userId);
                
                if (player != null && new[] {1,2,3,4}.Contains(typeId))
                {
                    var rewardName = "";
                    switch (typeId)
                    {
                        case 1:
                            player.ChangeData(DataType.URIDIUM, amount);
                            rewardName = "uridium";
                            break;
                        case 2:
                            player.ChangeData(DataType.CREDITS, amount);
                            rewardName = "credits";
                            break;
                        case 3:
                            player.ChangeData(DataType.HONOR, amount);
                            rewardName = "honor";
                            break;
                        case 4:
                            player.ChangeData(DataType.EXPERIENCE, amount);
                            rewardName = "experience";
                            break;
                    }
                    player.SendPacket($"0|A|STD|You got {amount} {rewardName} from {gameSession.Player.Name}.");
                    Send($"dq%{player.Name} has got {amount} {rewardName} from you.#");
                    GameManager.ChatClients[player.Id].Send($"dq%You got {amount} {rewardName} from {gameSession.Player.Name}.#");
                }
            }
            else
            {
                if (!cmd.StartsWith("/"))
                {
                    foreach (var m in Filter)
                    {
                        if (message.Contains(m) && Permission == Permissions.NORMAL)
                        {
                            Send($"{ChatConstants.CMD_KICK_BY_SYSTEM}%#");
                            ShutdownConnection();
                            return;
                        }
                    }

                    foreach (var pair in GameManager.ChatClients.Values)
                    {
                        if (pair.ChatsJoined.Contains(Convert.ToInt32(roomId)))
                        {
                            var name = gameSession.Player.Name + (pair.Permission == Permissions.ADMINISTRATOR || pair.Permission == Permissions.CHAT_MODERATOR ? $" ({gameSession.Player.Id})" : "");
                            var color = (Permission == Permissions.ADMINISTRATOR || Permission == Permissions.CHAT_MODERATOR) ? "j" : "a";
                            messagePacket = $"{color}%" + roomId + "@" + name + "@" + message;

                            if (gameSession.Player.Clan.Tag != "")
                                messagePacket += "@" + gameSession.Player.Clan.Tag;

                            pair.Send(messagePacket + "#");
                        }
                    }
                }
            }
        }

        #region Connection

        public void ShutdownConnection()
        {
            try
            {
                if (Socket.IsBound && Socket.Connected)
                {
                    Socket.Shutdown(SocketShutdown.Both);
                    Socket.Close();
                    Socket.Dispose();
                }

                var value = this;
                GameManager.ChatClients.TryRemove(UserId, out value);
            }
            catch (Exception e)
            {
                Out.WriteLine("ShutdownConnection() void exception: " + e, "ChatClient.cs");
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                if (Socket == null) return;

                var bytesRead = Socket.EndReceive(ar);
                var content = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (content.Trim() == "") return;

                var packet = Encoding.UTF8.GetString(buffer, 0, bytesRead).Replace("\n", "");
                if (packet.StartsWith("<policy-file-request/>"))
                {
                    const string policyPacket = "<?xml version=\"1.0\"?>\r\n" +
                            "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                            "<cross-domain-policy>\r\n" +
                            "<allow-access-from domain=\"*\" to-ports=\"*\" />\r\n" +
                            "</cross-domain-policy>";

                    Write(Encoding.UTF8.GetBytes(policyPacket + (char)0x00));
                }
                else { Execute(content); }

                Socket.BeginReceive(buffer, 0, buffer.Length, 0, ReadCallback, this);
            }
            catch
            {
                //ignore 
            }
        }

        public void Send(String data)
        {
            try
            {
                if (!Socket.Connected) return;
                Write(Encoding.UTF8.GetBytes(data + (char)0x00));
            }
            catch (Exception e)
            {
                Out.WriteLine("Send void exception: " + e, "ChatClient.cs");
            }
        }

        private void Write(byte[] byteArray)
        {
            if (!Socket.IsBound && !Socket.Connected) new Exception("Unable to write. Socket is not bound or connected.");
            try
            {
                Socket.BeginSend(byteArray, 0, byteArray.Length, SocketFlags.None, null, null);
            }
            catch (Exception e)
            {
                new Exception("Something went wrong writting on the socket.\n" + e.Message);
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                var handler = (Socket)ar.AsyncState;
                handler.EndSend(ar);
            }
            catch (Exception e)
            {
                Out.WriteLine(e.Message);
            }
        }
        #endregion
    }
}
