using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ow.Utils;

namespace Ow.Net
{
    class GameServer
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static int Port = 8080;

        public static void StartListening()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Port);

            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    allDone.Reset();

                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Logger.Log("error_log", $"- [GameServer.cs] StartListening void exception: {e}");
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                allDone.Set();

                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                new GameClient(handler);
            } 
            catch (Exception e)
            {
                Logger.Log("error_log", $"- [GameServer.cs] AcceptCallback void exception: {e}");
            }
        }
    }
}