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
            if (ChatClients.ContainsKey(chatClient.UserId)) return;
            var gameSession = GameManager.GetGameSession(chatClient.UserId);
            gameSession.Chat = chatClient;
            ChatClients.TryAdd(chatClient.UserId, chatClient);
        }

        public static void RemoveChatClient(Chat.ChatClient chatClient)
        {
            if (ChatClients.ContainsKey(chatClient.UserId))
            {
                var gameSession = GameManager.GetGameSession(chatClient.UserId);
                gameSession.Chat = null;
                ChatClients.TryRemove(chatClient.UserId, out chatClient);
            }
        }
    }
}
