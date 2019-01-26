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
                string received = Encoding.UTF8.GetString(buffer, 0, count);


                //JObject.Parse(received)["UserId"] //örnek json parse //int için bunu tostring yapıp convert to int 32 ile inte dönüştürüyoruz

                switch (JObject.Parse(received)["Action"].ToString())
                {
                    case "SendMessage":
                        SendMessage(Convert.ToInt32(JObject.Parse(received)["UserId"].ToString()), JObject.Parse(received)["Message"].ToString());
                        break;
                    case "ChangeShip":
                        ChangeShip(Convert.ToInt32(JObject.Parse(received)["UserId"].ToString()), Convert.ToInt32(JObject.Parse(received)["ShipId"].ToString()));
                        break;
                    case "UpdateStatus":
                        UpdateStatus(Convert.ToInt32(JObject.Parse(received)["UserId"].ToString()), JObject.Parse(received)["Status"].ToString());
                        break;
                }

            }
            catch (Exception e)
            {
                Out.WriteLine(e.Message, "SocketServer ERROR");
            }
        }

        public static void SendMessage(int userId, string message)
        {
            var player = GameManager.GetPlayerById(userId);

            if (player != null)
                player.SendPacket($"0|A|STD|{message}");
        }

        public static void ChangeShip(int userId, int shipId)
        {
            var player = GameManager.GetPlayerById(userId);

            if (player != null)
            {
                /*
                 * TODO
                var oldShipId = player.Ship.Id;
                var oldSkill = oldShipId == 63 ? 1 : oldShipId == 64 ? 2 : oldShipId == 65 ? 3 : oldShipId == 66 ? 4 : oldShipId == 67 ? 5 : 0;

                player.SendPacket("0|SD|S|" + oldSkill + "|0");
                */

                player.ChangeShip(shipId);
            }
        }

        public static void UpdateStatus(int userId, string status)
        {
            var player = GameManager.GetPlayerById(userId);

            if (player != null)
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
}