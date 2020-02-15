using Ow.Chat;
using Ow.Game.Events;
using Ow.Game.Objects;
using Ow.Game.Ticks;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net;
using Ow.Net.netty.commands;
using Ow.Utils;
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

        public Player Player { get; set; }
        public GameClient Client { get; set; }

        public DateTime LastActiveTime = new DateTime();
        public bool InProcessOfDisconnection = false;
        public DateTime EstDisconnectionTime = new DateTime();

        public GameSession(Player player)
        {
            Player = player;
            Program.TickManager.AddTick(this);
        }

        public void Tick()
        {
            if (LastActiveTime.AddMinutes(3) < DateTime.Now && !Player.AttackingOrUnderAttack(10))
                Disconnect(DisconnectionType.INACTIVITY);
            if (EstDisconnectionTime < DateTime.Now && InProcessOfDisconnection)
                Disconnect(DisconnectionType.NORMAL);
        }

        private void PrepareForDisconnect()
        {
            try
            {
                Player.LastCombatTime = DateTime.Now.AddSeconds(-999);
                Player.Group?.Leave(Player);
                Player.DisableAttack(Player.Settings.InGameSettings.selectedLaser);
                Duel.RemovePlayer(Player);
                Program.TickManager.RemoveTick(Player);
                Player.Spacemap.RemoveCharacter(Player);
                Player.Deselection();
            }
            catch (Exception e)
            {
                Out.WriteLine("PrepareForDisconnect void exception: " + e, "GameSession.cs");
                Logger.Log("error_log", $"- [GameSession.cs] PrepareForDisconnect void exception: {e}");
            }     
        }

        public void Disconnect(DisconnectionType dcType)
        {
            try
            {
                Player.Group?.UpdateTarget(Player, new List<command_i3O> { new GroupPlayerDisconnectedModule(true) });
                Player.UpdateCurrentCooldowns();
                Player.SaveSettings();
                QueryManager.SavePlayer.Information(Player);
                QueryManager.SavePlayer.Boosters(Player);

                Player.Storage.InRangeAssets.Clear();
                Player.Storage.InRangeObjects.Clear();
                Player.InRangeCharacters.Clear();

                InProcessOfDisconnection = true;

                if (dcType == DisconnectionType.SOCKET_CLOSED)
                {
                    EstDisconnectionTime = DateTime.Now.AddMinutes(2);
                    return;
                }

                foreach (var session in GameManager.GameSessions.Values)
                {
                    if (session != null)
                    {
                        var player = session.Player;
                        if (player.Storage.GroupInvites.ContainsKey(Player.Id))
                        {
                            player.Storage.GroupInvites.Remove(Player.Id);
                            player.SendCommand(GroupRemoveInvitationCommand.write(Player.Id, player.Id, GroupRemoveInvitationCommand.REVOKE));
                        }
                    }
                }

                PrepareForDisconnect();
                Client.Close();
                InProcessOfDisconnection = false;
                Program.TickManager.RemoveTick(this);
                GameManager.GameSessions.TryRemove(Player.Id, out var gameSession);

                Console.Title = $"DarkOrbit | {GameManager.GameSessions.Count} users online";
            }
            catch (Exception e)
            {
                Out.WriteLine("Disconnect void exception: " + e, "GameSession.cs");
                Logger.Log("error_log", $"- [GameSession.cs] Disconnect void exception: {e}");
            }
        }
    }
}
