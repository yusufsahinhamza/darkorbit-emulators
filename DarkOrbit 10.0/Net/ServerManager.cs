using Ow.Game.Events;
using Ow.Managers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ow.Net
{
    static class ServerManager
    {
        public static ConcurrentDictionary<int, GameClient> GameClients = new ConcurrentDictionary<int, GameClient>();
        public static ConcurrentDictionary<int, Chat.ChatClient> ChatClients = new ConcurrentDictionary<int, Chat.ChatClient>();

        public static void Start()
        {
            EventManager.InitiateEvents();
            Task.Factory.StartNew(GameServer.StartListening);
            Task.Factory.StartNew(ChatServer.StartListening);
            Task.Factory.StartNew(SocketServer.StartListening);
        }

        public static void AddGameClient(GameClient gameClient)
        {
            if (GameClients.ContainsKey(gameClient.UserId)) return;
            GameClients.TryAdd(gameClient.UserId, gameClient);
        }

        public static void RemoveGameClient(GameClient gameClient)
        {
            if (GameClients.ContainsKey(gameClient.UserId))
                GameClients.TryRemove(gameClient.UserId, out gameClient);
        }

        public static void AddChatClient(Chat.ChatClient chatClient)
        {
            if (ChatClients.ContainsKey(chatClient.GameSession.Player.Id)) return;
            chatClient.GameSession.Chat = chatClient;
            ChatClients.TryAdd(chatClient.GameSession.Player.Id, chatClient);
        }

        public static void RemoveChatClient(Chat.ChatClient chatClient)
        {
            if (ChatClients.ContainsKey(chatClient.GameSession.Player.Id))
            {
                chatClient.GameSession.Chat = null;
                ChatClients.TryRemove(chatClient.GameSession.Player.Id, out chatClient);
            }
        }
    }
}
