using Ow.Chat;
using Ow.Game.Objects;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net;
using Ow.Net.netty.commands;
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
            if (LastActiveTime.AddMinutes(3) < DateTime.Now && Player.AttackingOrUnderAttack(10))
                Disconnect(DisconnectionType.INACTIVITY);
            if (EstDisconnectionTime < DateTime.Now && InProcessOfDisconnection)
                Disconnect(DisconnectionType.NORMAL);
        }

        private void PrepareForDisconnect()
        {
            Player.Group?.UpdatePlayer(Player, new List<command_i3O> { new GroupPlayerActiveModule(false) });
            Player.Pet.Deactivate();
            Player.DisableAttack(Player.Settings.InGameSettings.selectedLaser);
            QueryManager.SavePlayer.Information(Player);
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
            if (EventManager.JackpotBattle.Active && Player.Spacemap.Id == EventManager.JackpotBattle.Spacemap.Id && EventManager.JackpotBattle.Players.ContainsKey(Player.Id))
            {
                var thisPlayer = Player;
                EventManager.JackpotBattle.Players.TryRemove(Player.Id, out thisPlayer);
                GameManager.SendPacketToMap(EventManager.JackpotBattle.Spacemap.Id, "0|LM|ST|SLE|" + EventManager.JackpotBattle.Players.Count);
            }

            InProcessOfDisconnection = true;
            Player.UpdateCurrentCooldowns();
            if (dcType == DisconnectionType.SOCKET_CLOSED)
            {
                EstDisconnectionTime = DateTime.Now.AddSeconds(60);
                return;
            }

            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                mySqlClient.ExecuteNonQuery($"UPDATE player_accounts SET online = 0 WHERE userID = {Player.Id}");
            }

            foreach (var gameSessions in GameManager.GameSessions.Values)
            {           
                if (gameSessions != null)
                {
                    var player = gameSessions.Player;
                    if (player.Storage.GroupInvites.ContainsKey(Player.Id))
                    {
                        player.Storage.GroupInvites.Remove(Player.Id);
                        player.SendCommand(GroupRemoveInvitationCommand.write(Player.Id, player.Id, GroupRemoveInvitationCommand.REVOKE));
                    }
                }               
            }

            PrepareForDisconnect();
            Client.Disconnect();
            InProcessOfDisconnection = false;
            var gameSession = this;
            GameManager.GameSessions.TryRemove(Player.Id, out gameSession);
            Program.TickManager.RemoveTick(this);

            Console.Title = $"RisingBattle | {GameManager.GameSessions.Count} users online";
        }
    }
}
