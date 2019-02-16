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

            var tickId = -1;
            Program.TickManager.AddTick(GameSession.Player, out tickId);
            GameSession.Player.TickId = tickId;
        }

        public static void SendPlayerItems(Player player)
        {
            player.SendPacket("0|t");
            player.SendCommand(player.GetShipInitializationCommand());

            if (player.Title != "")
                player.SendPacket("0|n|t|" + player.Id + "|1|" + player.Title + "");

            player.DroneManager.UpdateDrones();

            player.SendPacket("0|S|CFG|" + player.CurrentConfig);
            player.SendPacket("0|A|BK|100"); //yeşil kutu miktarı
            player.SendPacket("0|A|JV|100"); //atlama kuponu miktarı
            player.SendPacket("0|n|ssi|0|0|0");

            player.SendPacket("0|ps|nüscht");
            player.SendPacket("0|ps|blk|" + Convert.ToInt32(player.Settings.InGameSettings.blockedGroupInvites));

            if (player.Group != null)
                GroupSystem.GroupInitializationCommand(player.GetGameSession().Player);

            player.Spacemap.SendObjects(player);
            player.Spacemap.SendPlayers(player);
            player.Spacemap.OnPlayerMovement(player);

            player.CheckAbilities(player);

            player.SettingsManager.SendRemoveWindows();
            player.SendCommand(PetInitializationCommand.write(true, true, true));
            player.UpdateStatus();
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
            player.SettingsManager.SendSlotBarItems();
            player.SettingsManager.SendUserSettingsCommand();

        }
    }
}
