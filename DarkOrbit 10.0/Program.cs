using MySQLManager.Database.Session_Details.Interfaces;
using Newtonsoft.Json;
using Ow.Chat;
using Ow.Game;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net;
using Ow.Net.netty;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ow
{
    class Program
    {
        public static TickManager TickManager = new TickManager();

        public static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                CheckMySQLConnection();
                LoadDatabase();
                InitiateServer();
                KeepAlive();
            } 
            catch(Exception e)
            {
                Out.WriteLine("Main void exception: " + e, "Program.cs");
                Logger.Log("error_log", $"- [Program.cs] Main void exception: {e}");
            }
        }

        private static void KeepAlive()
        {
            while (true)
            {
                var l = Console.ReadLine();
                if (l != "" && l.StartsWith("/"))
                    ExecuteCommand(l);
            }
        }

        public static bool CheckMySQLConnection()
        {
            if (!SqlDatabaseManager.Initialized)
            {
                int tries = 0;
                TRY:
                try
                {
                    SqlDatabaseManager.Initialize();
                    Out.WriteLine("DarkOrbit is started..", "EMU");
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("MYSQL Connection failed: " + e);
                    Out.WriteLine("MYSQL Connection failed!");
                    if (tries < 6)
                    {
                        Out.WriteLine("Trying to reconnect in .. " + tries + " seconds.");
                        Thread.Sleep(tries * 1000);
                        tries++;
                        goto TRY;
                    } else Environment.Exit(0);
                }
            }
            return false;
        }

        public static void LoadDatabase()
        {
            QueryManager.LoadClans();
            QueryManager.LoadShips();
            QueryManager.LoadMaps();
        }

        public static void InitiateServer()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
            Handler.AddCommands();
            Room.AddRooms();
            EventManager.InitiateEvents();
            StartListening();
        }

        public static void StartListening()
        {
            Task.Factory.StartNew(GameServer.StartListening);
            Out.WriteLine("Listening on port " + GameServer.Port + ".", "GameServer");
            Task.Factory.StartNew(ChatServer.StartListening);
            Out.WriteLine("Listening on port " + ChatServer.Port + ".", "ChatServer");
            Task.Factory.StartNew(SocketServer.StartListening);
            Out.WriteLine("Listening on port " + SocketServer.Port + ".", "SocketServer");

            Task.Factory.StartNew(TickManager.Tick);
        }

        public static void ExecuteCommand(string txt)
        {
            var packet = txt.Replace("/", "");
            var splitted = packet.Split(' ');
        
            switch (splitted[0])
            {
                case "restart":
                    GameManager.Restart(Convert.ToInt32(splitted[1]));
                    break;
                case "list_players":
                    foreach (var gameSession in GameManager.GameSessions.Values)
                    {
                        if (gameSession != null)
                            Out.WriteLine($"{gameSession.Player.Name} ({gameSession.Player.Id})");
                    }
                    break;
            }        
        }
    }
}