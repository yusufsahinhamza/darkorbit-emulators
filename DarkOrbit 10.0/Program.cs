using MySQLManager.Database.Session_Details.Interfaces;
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
using System.Threading;
using System.Threading.Tasks;

namespace Ow
{
    class Program
    {
        public static TickManager TickManager = new TickManager();

        public static void Main(string[] args)
        {
            CheckMySQLConnection();
            LoadDatabase();
            InitiateServer();
            KeepAlive();
        }

        private static void KeepAlive()
        {
            while (true)
            {
                var l = Console.ReadLine();
                if (l != "")
                {
                    if (l.StartsWith("/")) ExecuteCommand(l);
                }
            }
        }

        public static bool CheckMySQLConnection()
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
            Handler.AddCommands();
            Room.AddRooms();
            ServerManager.Start();
            TickManager.Tick();
        }

        public static void ExecuteCommand(string txt)
        {
            var packet = txt.Replace("/", "");
            var splitted = packet.Split(' ');

            /*
            switch (splitted[0])
            {
                case "destroy":
                    var player = GameManager.GetPlayerById(Convert.ToInt32(splitted[1]));

                    if (player != null)
                        player.Destroy(player, Game.DestructionType.PLAYER);
                    break;
            }
            */
        }
    }
}