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

namespace Ow.Chat
{
    public enum Permissions
    {
        ADMINISTRATOR = 1,
        CHAT_MODERATOR = 2
    }

    class ChatClient
    {
        public Socket Socket { get; set; }
        public bool SocketClosed { get; set; }
        private readonly byte[] buffer = new byte[2048];

        public int UserId { get; set; }
        public string Clan { get; set; }
        public Permissions Permission { get; set; }
        public List<Int32> ChatsJoined = new List<Int32>();

        public static List<string> Filter = new List<string>
        {
            "orospu",
            "ananı"
        };

        public ChatClient(Socket Socket)
        {
            this.Socket = Socket;
            try
            {
                if (!Socket.IsBound && !Socket.Connected) throw new Exception("Unable to read. Socket is not bound or connected.");

                this.Socket.BeginReceive(buffer, 0, buffer.Length, 0, ReadCallback, this);
            }
            catch (Exception e)
            {
                Out.WriteLine("Error: " + e.Message, "", ConsoleColor.Red);
            }
        }

        public void Execute(string message)
        {
            string[] packet = message.Split(ChatConstants.MSG_SEPERATOR);
            switch (packet[0])
            {
                case ChatConstants.CMD_USER_LOGIN:
                    var loginPacket = message.Replace("@", "%").Split('%');
                    UserId = Convert.ToInt32(loginPacket[3]);

                    /*
                    if (Program.IsUserBanned(Username))
                    {
                        _handler.Shutdown(SocketShutdown.Both);
                        _handler.Close();
                        _handler = null;
                        return;
                    }
                    */

                    var gameSession = GameManager.GetGameSession(UserId);

                    if (gameSession != null)
                    {
                        Clan = loginPacket[7] == "noclan" ? "" : gameSession.Player.Clan.Tag;
                        Permission = (Permissions)QueryManager.GetChatPermission(gameSession.Player.Id);

                        if (GameManager.ChatClients.ContainsKey(UserId))
                            GameManager.ChatClients[gameSession.Player.Id]?.ShutdownConnection();

                        GameManager.ChatClients.TryAdd(gameSession.Player.Id, this);

                        Send("bv%" + gameSession.Player.Id + "#");
                        var servers = Room.Rooms.Aggregate(String.Empty, (current, chat) => current + chat.Value.ToString());
                        servers = servers.Remove(servers.Length - 1);
                        Send("by%" + servers + "#");
                        ChatsJoined.Add(Room.Rooms.FirstOrDefault().Value.Id);
                    }
                    break;
                case ChatConstants.CMD_USER_MSG:
                    SendMessage(message);
                    break;
                case ChatConstants.CMD_USER_JOIN:
                    var newchat = Convert.ToInt32(message.Split('%')[2].Split('@')[0]);
                    if (Room.Rooms.ContainsKey(newchat))
                    {
                        if (!ChatsJoined.Contains(newchat))
                            ChatsJoined.Add(newchat);
                    }
                    else
                    {
                        /*
                        var inviterPlayer = GameManager.Players.Values.FirstOrDefault(x => x.DuelInvites.ContainsValue(Player));
                        AcceptDuel(inviterPlayer);
                        */
                    }
                    break;
            }
        }

        public void AcceptDuel(Player inviterPlayer)
        {
            var gameSession = GameManager.GetGameSession(UserId);
            if (inviterPlayer != null)
            {
                var roomId = inviterPlayer.Storage.DuelInvites.Keys.FirstOrDefault(x => inviterPlayer.Storage.DuelInvites.ContainsValue(gameSession.Player));
                Send("fr%" + roomId + "#");

                if (gameSession.Player.Settings.InGameSettings.inEquipZone && inviterPlayer.Settings.InGameSettings.inEquipZone)
                {
                    var position1 = new Position(3700, 3200);
                    var position2 = new Position(6400, 3200);

                    gameSession.Player.Jump(101, position1);
                    inviterPlayer.Jump(101, position2);
                    var duel = new Duel(gameSession.Player, inviterPlayer);
                    gameSession.Player.Storage.Duel = duel;
                    inviterPlayer.Storage.Duel = duel;
                }
            }
        }

        public void SendMessage(string content)
        {
            var gameSession = GameManager.GetGameSession(UserId);

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
                var userName = message.Split(' ')[1];

                if (userName.ToLower() == gameSession.Player.Name.ToLower())
                {
                    Send("fk%" + roomId + "@" + "Kendine fısıldayamazsın." + "#");
                    return;
                }

                if (GameManager.GetPlayerByName(userName) == null)
                {
                    Send("ct%#");
                    return;
                }

                foreach (var user in GameManager.ChatClients.Values)
                {
                    var userSession = GameManager.GetGameSession(user.UserId);
                    if (string.Equals(userSession.Player.Name.ToLower(), userName.ToLower(), StringComparison.CurrentCultureIgnoreCase) && user.ChatsJoined.Contains(Convert.ToInt32(roomId)))
                    {
                        message = message.Remove(0, userName.Length + 4);
                        user.Send("cv%" + gameSession.Player.Name + "@" + message + "#");
                        Send("cw%" + userName + "@" + message + "#");
                    }
                }
            }
            else if (cmd == "/kick" && (Permission == Permissions.ADMINISTRATOR || Permission == Permissions.CHAT_MODERATOR))
            {
                /*
                var userName = message.Split(' ')[1];
                var player = GameManager.GetPlayerByName(userName);

                if (player != null && player.GameSession.Chat.Permission != Permissions.ADMINISTRATOR && userName != GameSession.Player.Name)
                {
                    var chatClient = player.GameSession.Chat;
                    if (chatClient != null)
                    {
                        chatClient.Send("as%#");
                        ShutdownConnection(chatClient);
                    }
                }
                */
            }
            else if (cmd == "/duel" && Permission == Permissions.ADMINISTRATOR)
            {
                /*
                var userName = message.Split(' ')[1];
                var duelPlayer = GameManager.GetPlayerByName(userName);

                if (duelPlayer == null || duelPlayer == GameSession.Player) return;

                var duelId = Randoms.CreateRandomID();

                Send($"cr%{duelPlayer.Name}#");
                GameSession.Player.Storage.DuelInvites.Add(duelId, duelPlayer);

                var chatClient = duelPlayer.GameSession.Chat;
                chatClient.Send("cj%" + duelId + "@" + "Room" + "@" + GameSession.Player.Id + "@" + GameSession.Player.Name + "#");
                */
            }
            else if (cmd == "/a" && Permission == Permissions.ADMINISTRATOR)
            {
                /*
                if (Player.DuelUser != null)
                {
                    if (Player.IsInEquipZone && Player.DuelUser.IsInEquipZone)
                    {
                        Player.Jump(101, new Position(700, 3000));
                        Player.DuelUser.Jump(101, new Position(9500, 3000));
                    }
                }
                else
                {
                    Send($"dq%Herhangi bir meydan okuman bulunmuyor.#");
                }
                */
            }
            else if (cmd == "/r" && Permission == Permissions.ADMINISTRATOR)
            {
                /*
                if (Player.DuelUser != null)
                {
                    var chatClient = Player.DuelUser.GetGameSession().Chat;
                    Send($"dq%{Player.DuelUser.Name} adlı oyuncunun meydan okumasını reddettin.#");
                    chatClient.Send($"dq%{Player.Name} adlı oyuncu meydan okumanı reddetti.#");
                    Player.DuelUser = null;
                }
                else
                {
                    Send($"dq%Herhangi bir meydan okuman bulunmuyor.#");
                }
                */
            }
            else if (cmd == "/msg" && Permission == Permissions.ADMINISTRATOR)
            {
                var msg = message.Remove(0, 4);
                GameManager.SendPacketToAll($"0|A|STD|{msg}");
            }
            else if (cmd == "/patlat" && Permission == Permissions.ADMINISTRATOR)
            {
                var userName = message.Split(' ')[1];

                var player = GameManager.GetPlayerByName(userName);

                if (player != null)
                    player.Destroy(gameSession.Player, Game.DestructionType.PLAYER);
            }
            else if (cmd == "/ship" && Permission == Permissions.ADMINISTRATOR)
            {
                var shipId = Convert.ToInt32(message.Split(' ')[1]);

                gameSession.Player.Ship = GameManager.GetShip(shipId);
                gameSession.Player.Jump(gameSession.Player.Spacemap.Id, gameSession.Player.Position);
            }
            else if (cmd == "/jump" && Permission == Permissions.ADMINISTRATOR)
            {
                var mapId = Convert.ToInt32(message.Split(' ')[1]);
                gameSession.Player.Jump(mapId, new Position(0, 0));
            }
            else if (cmd == "/speed+" && Permission == Permissions.ADMINISTRATOR)
            {
                var speed = Convert.ToInt32(message.Split(' ')[1]);
                gameSession.Player.SetSpeedBoost(speed);
            }
            else if (cmd == "/god" && Permission == Permissions.ADMINISTRATOR)
            {
                var mod = message.Split(' ')[1];
                switch (mod)
                {
                    case "on":
                        gameSession.Player.Storage.GodMode = true;
                        break;
                    case "off":
                        gameSession.Player.Storage.GodMode = false;
                        break;
                }
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
                GameManager.SendPacketToAll($"0|A|STD|System preparing for Jackpot Battle!");
                EventManager.JackpotBattle.Start();
            }
            else if (cmd == "/ban" && (Permission == Permissions.ADMINISTRATOR || Permission == Permissions.CHAT_MODERATOR))
            {
                /*
                0 CHAT BAN
                1 OYUN BANI
                */
                var playerName = message.Split(' ')[1];
                var typeId = Convert.ToInt32(message.Split(' ')[2]);
                var day = Convert.ToInt32(message.Split(' ')[3]);
                var reason = message.Remove(0, playerName.Length + typeId.ToString().Length + day.ToString().Length + 8);

                if (typeId == 1 && Permission == Permissions.CHAT_MODERATOR) return;

                if (typeId == 0 || typeId == 1)
                {
                    var player = GameManager.GetPlayerByName(playerName);
                    if (player == null) return;

                    //player.SendPacket($"0|A|STD|{day} gün yasaklandın.");
                    GameManager.ChatClients[player.Id]?.ShutdownConnection();
                    QueryManager.ChatFunctions.AddBan(player.Id, gameSession.Player.Id, reason, typeId, (DateTime.Now.AddDays(day)).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
            }
            else if (cmd == "/restart" && Permission == Permissions.ADMINISTRATOR)
            {
                var seconds = Convert.ToInt32(message.Split(' ')[1]);
                GameManager.Restart(seconds);
            }
            else if (cmd == "/users")
            {
                var users = GameManager.GameSessions.Values.Aggregate(String.Empty, (current, user) => current + user.Player.Name + ", ");
                users = users.Remove(users.Length - 2);

                Send($"dq%Users online {GameManager.GameSessions.Count}: {users}#");
            }
            else
            {
                if (!cmd.StartsWith("/"))
                {
                    foreach (var m in Filter)
                    {
                        if (message.Contains(m))
                            Console.WriteLine("BAN, mesaj = " + m);
                    }

                    foreach (var pair in GameManager.ChatClients.Values)
                    {
                        if (pair.ChatsJoined.Contains(Convert.ToInt32(roomId)))
                        {
                            if (Permission == Permissions.ADMINISTRATOR || Permission == Permissions.CHAT_MODERATOR)
                                messagePacket = "j%" + roomId + "@" + gameSession.Player.Name + "@" + message;
                            else
                                messagePacket = "a%" + roomId + "@" + gameSession.Player.Name + "@" + message;

                            if (Clan != "")
                                messagePacket += "@" + Clan;

                            pair.Send(messagePacket + "#");
                        }
                    }
                }
            }
        }

        #region Connection

        public void ShutdownConnection()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
            Socket = null;
            var value = this;
            GameManager.ChatClients.TryRemove(UserId, out value);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
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
                else
                {
                    Execute(content);
                }

                if (Socket != null && Socket.Connected && Socket.IsBound)
                {
                    Socket.BeginReceive(buffer, 0, buffer.Length, 0,
                        ReadCallback, this);
                }
            }
            catch
            {
                //ignore 
            }
        }

        public void Send(String data)
        {
            if (Socket == null && !Socket.Connected) throw new Exception("Unable to write. Socket is not bound or connected.");
            try
            {
                var byteData = Encoding.UTF8.GetBytes(data + (char)0x00);
                Socket.BeginSend(byteData, 0, byteData.Length, 0,
                                       SendCallback, Socket);
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong writting on the socket.\n" + e.Message);
            }
        }

        private void Write(byte[] byteArray)
        {
            if (Socket == null && !Socket.Connected) throw new Exception("Unable to write. Socket is not bound or connected.");
            try
            {
                Socket.BeginSend(byteArray, 0, byteArray.Length, SocketFlags.None, null, null);
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong writting on the socket.\n" + e.Message);
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
