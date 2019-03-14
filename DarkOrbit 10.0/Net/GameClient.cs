using Ow.Game;
using Ow.Managers;
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

namespace Ow.Net
{
    class GameClient
    {
        public Socket Socket { get; set; }
        private readonly byte[] buffer = new byte[1024 * 3];
        public int UserId { get; set; }

        public GameClient(Socket Socket)
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

        #region Connection 
        public void Disconnect()
        {
            try
            {
                Close();
            }
            catch (Exception)
            {
                Out.WriteLine("Error disconnecting user from Game", "GAME", ConsoleColor.DarkRed);
            }
        }

        private void OnConnectionClosed()
        {
            var gameSession = GameManager.GetGameSession(UserId);
            if (gameSession != null)
                gameSession.Disconnect(GameSession.DisconnectionType.SOCKET_CLOSED);
        }

        public void Close()
        {
            try
            {
                if (Socket.IsBound)
                {
                    Socket.Shutdown(SocketShutdown.Both);
                    Socket.Close();
                    OnConnectionClosed();
                }
            }
            catch { /*ignored*/ }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                var bytesRead = Socket.EndReceive(ar);

                if (bytesRead <= 0)
                {
                    Close();
                    return;
                }

                byte[] bytes = new byte[bytesRead];
                Buffer.BlockCopy(buffer, 0, bytes, 0, bytesRead);

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
                    Handler.Execute(bytes, this);
                }

                if (Socket != null && Socket.Connected && Socket.IsBound)
                {
                    Socket.BeginReceive(buffer, 0, buffer.Length, 0,
                        ReadCallback, this);
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
        #endregion Connection
    }
}
