using Ow.Game;
using Ow.Game.Events;
using Ow.Game.Movements;
using Ow.Game.Objects;
using Ow.Game.Objects.Players;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class VersionRequestHandler
    {
        public static Player Player { get; set; }
        public GameSession GameSession { get; set; }

        public VersionRequestHandler(GameClient client, int userId)
        {
            client.UserId = userId;

            var gameSession = GameManager.GetGameSession(userId);
            if (gameSession != null)
            {
                Player = gameSession.Player;
                gameSession.Disconnect();
            }
            else
            {
                Player = QueryManager.GetPlayer(userId);
                Player.SetCurrentCooldowns();
            }

            if (Player != null) GameSession = CreateSession(client, Player);
            else
            {
                Console.WriteLine("Failed loading user ship / ShipInitializationHandler ERROR");
                return;
            }

            Execute();

            if (Player.Destroyed) Player.KillScreen(null, DestructionType.MISC, true);
            else
            {
                SendSettings(Player);
                SendPlayerItems(Player);
            }
        }

        private GameSession CreateSession(GameClient client, Player player)
        {
            return new GameSession(player)
            {
                Client = client,
                LastActiveTime = DateTime.Now
            };
        }

        public void Execute()
        {
            using (var mySqlClient = SqlDatabaseManager.GetClient())
            {
                mySqlClient.ExecuteNonQuery($"UPDATE player_accounts SET online = 1 WHERE userID = {Player.Id}");
            }

            if (GameSession == null) return;

            if (!GameManager.GameSessions.ContainsKey(Player.Id))
            {
                SetShip();
                SetSpacemap();
                GameManager.GameSessions.TryAdd(Player.Id, GameSession);
            }
            else
            {
                GameManager.GameSessions[Player.Id].Disconnect(GameSession.DisconnectionType.NORMAL);
                GameManager.GameSessions[Player.Id] = GameSession;
            }

            LoadPlayer();
        }

        private void SetShip()
        {
            var player = GameSession.Player;
            player.CurrentHitPoints = player.MaxHitPoints;
            player.CurrentShieldConfig1 = player.Equipment.Config1Shield;
            player.CurrentShieldConfig2 = player.Equipment.Config2Shield;

            if (player.RankId == 21)
            {
                //player.AddVisualModifier(new VisualModifierCommand(player.Id, VisualModifierCommand.GREEN_GLOW, 0, true));
            }
        }

        private void SetSpacemap()
        {
            var player = GameSession.Player;
            var mapId = player.FactionId == 1 ? 13 : player.FactionId == 2 ? 14 : 15;
            player.Spacemap = GameManager.GetSpacemap(mapId);
        }

        private void LoadPlayer()
        {
            Player.Spacemap.AddCharacter(Player);
            Player.SetPosition(Player.FactionId == 1 ? Position.MMOPosition : Player.FactionId == 2 ? Position.EICPosition : Position.VRUPosition);

            var tickId = -1;
            Program.TickManager.AddTick(GameSession.Player, out tickId);
            GameSession.Player.TickId = tickId;
        }

        public static void SendPlayerItems(Player player, bool isLogin = true)
        {
            player.SendCommand(player.GetShipInitializationCommand());

            if (player.Title != "")
                player.SendPacket("0|n|t|" + player.Id + "|1|" + player.Title + "");

            if (isLogin)
            {
                player.DroneManager.UpdateDrones();
                player.SendPacket("0|S|CFG|" + player.CurrentConfig);
                player.SendPacket("0|A|BK|0"); //yeşil kutu miktarı
                player.SendPacket("0|A|JV|0"); //atlama kuponu miktarı
            }
            else
            {
                player.SendPacket(player.DroneManager.GetDronesPacket());
                var droneFormationChangeCommand = DroneFormationChangeCommand.write(player.Id, (int)player.SettingsManager.SelectedFormation);
                player.SendCommand(droneFormationChangeCommand);
                player.SendCommandToInRangePlayers(droneFormationChangeCommand);
            }

            var spaceball = EventManager.Spaceball.Character;
            if (EventManager.Spaceball.Active && spaceball != null)
                player.SendPacket($"0|n|ssi|{spaceball.Mmo}|{spaceball.Eic}|{spaceball.Vru}");
            else
                player.SendPacket($"0|n|ssi|0|0|0");


            if (isLogin)
            {
                player.SendPacket("0|ps|nüscht");
                player.SendPacket("0|ps|blk|" + Convert.ToInt32(player.Settings.InGameSettings.blockedGroupInvites));

                if (player.Group != null)
                    GroupSystem.GroupInitializationCommand(player.GameSession.Player);
                else
                {
                    foreach (var userId in player.Storage.GroupInvites.Keys)
                        if (GameManager.GetPlayerById(userId) != null)
                            GroupSystem.GroupInviteCommand(GameManager.GetPlayerById(userId), player);
                }
            }

            if (!player.Premium)
                player.SendCommand(PetBlockUICommand.write(true));

            player.Spacemap.SendObjects(player);

            if (isLogin)
                player.Spacemap.SendPlayers(player);

            player.CheckAbilities(player);

            if (isLogin)
                player.SettingsManager.SendSlotBarItems();

            player.SettingsManager.SendRemoveWindows();

            if (isLogin)
            {
                player.SendCommand(PetInitializationCommand.write(true, true, true));
                player.UpdateStatus();
            }

            player.SendCurrentCooldowns();
            QueryManager.SavePlayer.Information(player);
        }


        public void CheckAbilities(Player player)
        {
            var sentinel = player.SkillManager.Sentinel;
            var diminisher = player.SkillManager.Diminisher;
            var spectrum = player.SkillManager.Spectrum;
            var venom = player.SkillManager.Venom;
            player.SendPacket($"0|SD|{(sentinel.Active ? "A" : "D")}|R|4|{player.Id}");
            player.SendPacket($"0|SD|{(diminisher.Active ? "A" : "D")}|R|2|{player.Id}");
            player.SendPacket($"0|SD|{(spectrum.Active ? "A" : "D")}|R|3|{player.Id}");
            player.SendPacket($"0|SD|{(venom.Active ? "A" : "D")}|R|5|{player.Id}");
        }

        public static void SendSettings(Player player)
        {
            player.SettingsManager.SendUserKeyBindingsUpdateCommand();
            player.SettingsManager.SendUserSettingsCommand();
        }
    }
}
