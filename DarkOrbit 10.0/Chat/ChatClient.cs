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

        public Player Player { get; set; }
        public int UserId { get; set; }
        public string SessionId { get; set; }
        public string Username { get; set; }
        public string Clan { get; set; }
        public Permissions Permission { get; set; }
        public List<Int32> ChatsJoined = new List<Int32>();

        public ChatClient(Socket Socket)
        {
            this.Socket = Socket;
            try
            {
                if (!Socket.IsBound && !Socket.Connected) throw new Exception("Unable to read. Socket is not bound or connected.");

                this.Socket.BeginReceive(buffer, 0, buffer.Length, 0,
                    ReadCallback, this);
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
                    Player = GameManager.GetPlayerById(UserId);
                    Clan = loginPacket[7] == "noclan" ? "" : Player.GetClanTag();
                    Username = Player.Name;
                    SessionId = loginPacket[4];
                    Permission = (Permissions)QueryManager.GetChatPermission(Player.Id);

                    if (ServerManager.ChatClients.ContainsKey(UserId))
                    {
                        ServerManager.ChatClients[UserId].Socket.Shutdown(SocketShutdown.Both);
                        ServerManager.ChatClients[UserId].Socket.Close();
                        ServerManager.ChatClients[UserId].Socket = null;
                        ServerManager.RemoveChatClient(this);
                    }
                    ServerManager.AddChatClient(this);

                    Send("bv%" + UserId + "#");
                    var servers = Room.Rooms.Aggregate(String.Empty, (current, chat) => current + chat.Value.ToString());
                    servers = servers.Remove(servers.Length - 1);
                    Send("by%" + servers + "#");
                    ChatsJoined.Add(Room.Rooms.FirstOrDefault().Value.Id);
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
            if (inviterPlayer != null)
            {
                var roomId = inviterPlayer.DuelInvites.Keys.FirstOrDefault(x => inviterPlayer.DuelInvites.ContainsValue(Player));
                Send("fr%" + roomId + "#");

                if (Player.IsInEquipZone && inviterPlayer.IsInEquipZone)
                {
                    var position1 = new Position(3700, 3200);
                    var position2 = new Position(6400, 3200);

                    Player.Jump(101, position1);
                    inviterPlayer.Jump(101, position2);
                    var duel = new Duel(Player, inviterPlayer);
                    Player.Duel = duel;
                    inviterPlayer.Duel = duel;
                }
            }
        }

        public void SendMessage(string content)
        {
            Player.GetGameSession().LastActiveTime = DateTime.Now;

            string messagePacket = "";

            var packet = content.Replace("@", "%").Split('%');
            var roomId = packet[1];
            var message = packet[2];

            var cmd = message.Split(' ')[0];
            if (message.StartsWith("/reconnect"))
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                Socket = null;
                ServerManager.RemoveChatClient(this);
            }
            else if (message.StartsWith("/users"))
            {
                var users = ServerManager.ChatClients.Values.Aggregate(String.Empty, (current, user) => current + user.Username + ", ");
                users = users.Remove(users.Length - 2);
                Send("fk%" + roomId + "@" + "Users online " + ServerManager.ChatClients.Count + ": " + users + "#");
            }
            else if (cmd == "/close")
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                Socket = null;
                ServerManager.RemoveChatClient(this);
            }
            else if (cmd == "/w")
            {
                foreach (var user in ServerManager.ChatClients.Values)
                {
                    var userName = message.Split(' ')[1];

                    if (userName.ToLower() == Username.ToLower())
                    {
                        Send("fk%" + roomId + "@" + "Kendine fısıldayamazsın." + "#");
                        return;
                    }

                    if (string.Equals(user.Username.ToLower(), userName.ToLower(), StringComparison.CurrentCultureIgnoreCase) && user.ChatsJoined.Contains(Convert.ToInt32(roomId)))
                    {
                        message = message.Remove(0, userName.Length + 4);
                        user.Send("cv%" + Username + "@" + message + "#");
                        Send("cw%" + userName + "@" + message + "#");
                    }
                }
            }
            else if (cmd == "/kick" && (Permission == Permissions.ADMINISTRATOR || Permission == Permissions.CHAT_MODERATOR))
            {
                var userName = message.Split(' ')[1];
                var player = GameManager.GetPlayerByName(userName);

                if(player != null && player.GetGameSession().Chat.Permission != Permissions.ADMINISTRATOR && userName != Player.Name)
                {
                    var chatClient = player.GetGameSession().Chat;
                    if (chatClient != null)
                    {
                        chatClient.Send("as%#");
                        chatClient.Socket.Shutdown(SocketShutdown.Both);
                        chatClient.Socket.Close();
                        chatClient.Socket = null;
                        ServerManager.RemoveChatClient(chatClient);
                    }
                }
            }
            else if (cmd == "/duel" && Permission == Permissions.ADMINISTRATOR)
            {
                var userName = message.Split(' ')[1];
                var duelPlayer = GameManager.GetPlayerByName(userName);

                if (duelPlayer == null || duelPlayer == Player) return;

                var duelId = Randoms.CreateRandomID();

                Send($"cr%{duelPlayer.Name}#");
                Player.DuelInvites.Add(duelId, duelPlayer);

                var chatClient = duelPlayer.GetGameSession().Chat;
                chatClient.Send("cj%" + duelId + "@" + "Room" + "@" + Player.Id + "@" + Username + "#");
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
                var msg = message.Split(' ')[1];
                GameManager.SendPacketToAll($"0|A|STD|{msg}");
            }
            else if (cmd == "/patlat" && Permission == Permissions.ADMINISTRATOR)
            {
                var userName = message.Split(' ')[1];

                var player = GameManager.GetPlayerByName(userName);

                if (player != null)
                    player.Destroy(Player, Game.DestructionType.PLAYER);
            }
            else if (cmd == "/ship" && Permission == Permissions.ADMINISTRATOR)
            {
                var shipId = Convert.ToInt32(message.Split(' ')[1]);

                Player.Ship = GameManager.GetShip(shipId);
                Player.Jump(Player.Spacemap.Id, Player.Position);
            }
            else if (cmd == "/jump" && Permission == Permissions.ADMINISTRATOR)
            {
                var mapId = Convert.ToInt32(message.Split(' ')[1]);
                Player.Jump(mapId, new Position(0, 0));
            }
            else if(cmd == "/speed+" && Permission == Permissions.ADMINISTRATOR)
            {
                var speed = Convert.ToInt32(message.Split(' ')[1]);
                Player.SetSpeedBoost(speed);
            }
            else if (cmd == "/god" && Permission == Permissions.ADMINISTRATOR)
            {
                var mod = message.Split(' ')[1];
                switch (mod)
                {
                    case "on":
                        Player.GodMode = true;
                        break;
                    case "off":
                        Player.GodMode = false;
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
                EventManager.JackpotBattle.Start();
            }
            else
            {
                if (!cmd.StartsWith("/"))
                {
                    foreach (var pair in ServerManager.ChatClients.Values)
                    {
                        if (pair.ChatsJoined.Contains(Convert.ToInt32(roomId)))
                        {
                            if (Permission == Permissions.ADMINISTRATOR)
                                messagePacket = "j%" + roomId + "@" + Username + "@" + message;
                            else if (Permission == Permissions.CHAT_MODERATOR)
                                messagePacket = "j%" + roomId + "@" + Username + "@" + message + "@3";
                            else
                                messagePacket = "a%" + roomId + "@" + Username + "@" + message;

                            if (Clan != "")
                                messagePacket += "@" + Clan;

                            pair.Send(messagePacket + "#");
                        }
                    }
                }
            }
        }

        #region Connection
        public void Close()
        {
            try
            {
                if (Socket.IsBound)
                {
                    Socket.Shutdown(SocketShutdown.Both);
                    Socket.Close();
                }
            }
            catch { /*ignored*/ }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                var bytesRead = Socket.EndReceive(ar);
                var content = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (bytesRead <= 0)
                {
                    Close();
                    return;
                }

                byte[] b = new byte[bytesRead];
                Array.Copy(buffer, b, bytesRead);

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

                Socket.BeginReceive(buffer, 0, buffer.Length, 0, ReadCallback, this);
            }
            catch
            {
                Close();
            }
        }

        public void Send(String data)
        {
            try
            {
                if (!Socket.IsBound && !Socket.Connected) throw new Exception("Unable to write. Socket is not bound or connected.");
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
            catch
            {
                Close();
            }
        }

        public void Send(byte[] data)
        {
            try
            {
                var gameSession = GameManager.GetGameSession(UserId);
                if (gameSession != null)
                {
                    if (gameSession.InProcessOfDisconnection) return;
                    if (!Socket.IsBound && !Socket.Connected) throw new Exception("Unable to write. Socket is not bound or connected.");

                    try
                    {
                        Socket.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Something went wrong writting on the socket.\n" + e.Message);
                    }
                }
            }
            catch
            {
                Close();
            }
        }

        private void Write(byte[] byteArray)
        {
            if (!Socket.IsBound && !Socket.Connected) throw new Exception("Unable to write. Socket is not bound or connected.");
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
