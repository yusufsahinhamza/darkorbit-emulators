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
                var json = JObject.Parse(Encoding.UTF8.GetString(buffer, 0, count));

                var player = GameManager.GetPlayerById(Convert.ToInt32(json["UserId"].ToString()));

                if (player.GameSession != null && player != null)
                {
                    switch (json["Action"].ToString())
                    {
                        case "SendMessage":
                            SendMessage(player, json["Message"].ToString());
                            break;
                        case "ChangeShip":
                            ChangeShip(player, Convert.ToInt32(json["ShipId"].ToString()));
                            break;
                        case "UpdateStatus":
                            UpdateStatus(player, json["Status"].ToString());
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                //ignore
            }
        }

        public static void SendMessage(Player player, string message)
        {
            player.SendPacket($"0|A|STD|{message}");
        }

        public static void ChangeShip(Player player, int shipId)
        {
            player.ChangeShip(shipId);
        }

        public static void UpdateStatus(Player player, string status)
        {
            player.Equipment.Config1Hitpoints = Convert.ToInt32(JObject.Parse(status)["Config1Hitpoints"].ToString());
            player.Equipment.Config1Damage = Convert.ToInt32(JObject.Parse(status)["Config1Damage"].ToString());
            player.Equipment.Config1Shield = Convert.ToInt32(JObject.Parse(status)["Config1Shield"].ToString());
            player.Equipment.Config1Speed = Convert.ToInt32(JObject.Parse(status)["Config1Speed"].ToString());
            player.Equipment.Config2Hitpoints = Convert.ToInt32(JObject.Parse(status)["Config2Hitpoints"].ToString());
            player.Equipment.Config2Damage = Convert.ToInt32(JObject.Parse(status)["Config2Damage"].ToString());
            player.Equipment.Config2Shield = Convert.ToInt32(JObject.Parse(status)["Config2Shield"].ToString());
            player.Equipment.Config2Speed = Convert.ToInt32(JObject.Parse(status)["Config2Speed"].ToString());

            player.DroneManager.UpdateDrones();
            player.UpdateStatus();
        }

    }
}