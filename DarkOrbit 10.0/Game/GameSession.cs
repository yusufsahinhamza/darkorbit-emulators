using Ow.Chat;
using Ow.Game.Objects;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Game
{
    class GameSession : Tick
    {
        public enum DisconnectionType
        {
            NORMAL,
            INACTIVITY,
            ADMIN,
            SOCKET_CLOSED,
            ERROR
        }

        public int TickId { get; set; }
        public Player Player { get; set; }
        public GameClient Client { get; set; }
        public ChatClient Chat { get; set; }

        public DateTime LastActiveTime = new DateTime();
        public bool InProcessOfDisconnection = false;
        public DateTime EstDisconnectionTime = new DateTime();

        public GameSession(Player player)
        {
            Player = player;

            var tickId = -1;
            Program.TickManager.AddTick(this, out tickId);
            TickId = tickId;
        }

        public void Tick()
        {
            if (LastActiveTime.AddMinutes(5) < DateTime.Now)
                Disconnect(DisconnectionType.INACTIVITY);
            if (EstDisconnectionTime < DateTime.Now && InProcessOfDisconnection)
                Disconnect(DisconnectionType.NORMAL);
        }

        private void PrepareForDisconnect()
        {
            Player.Pet.Deactivate();
            Player.DisableAttack(Player.SettingsManager.SelectedLaser);
            QueryManager.SavePlayer.Settings(Player);
            Player.Spacemap.RemoveCharacter(Player);
            Program.TickManager.RemoveTick(Player);       
        }

        public void Disconnect()
        {
            Client.Disconnect();
        }

        public void Disconnect(DisconnectionType dcType)
        {
            InProcessOfDisconnection = true;
            if (dcType == DisconnectionType.SOCKET_CLOSED)
            {
                EstDisconnectionTime = DateTime.Now.AddSeconds(30);
                return;
            }
            PrepareForDisconnect();
            Client.Disconnect();
            InProcessOfDisconnection = false;
            var gameSession = this;
            GameManager.GameSessions.TryRemove(Player.Id, out gameSession);
            Program.TickManager.RemoveTick(this);
        }
    }
}
