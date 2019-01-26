using Ow.Game;
using Ow.Game.Events;
using Ow.Game.Movements;
using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Net.netty.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class LoginRequestHandler
    {
        public static Player Player { get; set; }
        public GameSession GameSession { get; set; }

        public LoginRequestHandler(GameClient client, int userId)
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
                SendSettings(Player, !(gameSession != null && Player == gameSession.Player));
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
            if (GameSession == null) return;

            if (!GameManager.GameSessions.ContainsKey(Player.Id))
                GameManager.GameSessions.TryAdd(Player.Id, GameSession);
            else
            {
                GameManager.GameSessions[Player.Id].Disconnect(GameSession.DisconnectionType.NORMAL);
                GameManager.GameSessions[Player.Id] = GameSession;
            }

            SetShip();
            SetSpacemap();
            LoadTick();
        }

        private void SetShip()
        {
            var player = GameSession.Player;
            player.CurrentHitPoints = player.MaxHitPoints;
            player.CurrentShieldConfig1 = player.EquipmentManager.shieldCfg1;
            player.CurrentShieldConfig2 = player.EquipmentManager.shieldCfg2;

            if (player.RankId == 21)
            {
                //player.VisualModifiers.Add(new VisualModifierCommand(player.Id, VisualModifierCommand.LEONOV_EFFECT, 0, "", 0, true));
                //player.VisualModifiers.Add(new VisualModifierCommand(player.Id, VisualModifierCommand.DAMAGE_ICON, 0, "", 0, true));
            }
        }

        private void SetSpacemap()
        {
            var player = GameSession.Player;
            var mapId = player.FactionId == 1 ? 13 : player.FactionId == 2 ? 14 : 15;
            player.Spacemap = GameManager.GetSpacemap(mapId);
            player.Spacemap.AddCharacter(player);
        }

        private void LoadTick()
        {
            var tickId = -1;
            Program.TickManager.AddTick(GameSession.Player, out tickId);
            GameSession.Player.TickId = tickId;
        }

        public static void SendPlayerItems(Player player)
        {
            player.SendPacket("0|t");
            player.SendCommand(player.GetShipInitializationCommand());
            player.SendPacket("0|n|t|" + player.Id + "|0|title_1");
            player.SendPacket(player.GetDronesPacket());
            player.SendCommand(DroneFormationChangeCommand.write(player.Id, DroneManager.GetSelectedFormationId(player.SettingsManager.SelectedFormation)));
            player.SendPacket("0|S|CFG|" + player.CurrentConfig);

            player.SendPacket("0|A|BK|100"); //yeşil kutu miktarı
            player.SendPacket("0|A|JV|100"); //atlama kuponu miktarı
            player.SendPacket("0|n|ssi|0|0|0");

            var priceList = new List<JumpCPUPriceMappingModule>();
            var price = new List<int>();
            price.Add(1);
            price.Add(14);
            price.Add(15);
            price.Add(16);
            priceList.Add(new JumpCPUPriceMappingModule(0, price));
            player.SendCommand(JumpCPUUpdateCommand.write(priceList));

            player.SendCommand(CpuInitializationCommand.write(true, false));

            player.BoosterManager.Initiate();
            //player.SendPacket("0|A|JCPU|S|20|1"); //ATLAMA GERİ SAYIM
       
            player.SendCommand(SetSpeedCommand.write(player.Speed, player.Speed));

            player.Spacemap.SendObjects(player);
            player.Spacemap.SendPlayers(player);

            //player.SendCommand(VideoWindowCreateCommand.write(1, "l", true, new List<string> { "start_head", "tutorial_video_msg_reward_premium_intro" }, 1, 1));    

            //player.SendPacket("0|n|KSMSG|start_head");
            

            //player.SendCommand(command_e4W.write(1, "", new class_oS(0, 0, 0, 0, 0, 0, 0, 0, 0, 0), "", new class_s16(1, class_s16.varC3p), 1));
            player.SendCommand(UbaWindowInitializationCommand.write(new class_NQ(), 0));

            var ht = new List<UbaHtModule>();
            var j3s = new List<Ubaj3sModule>();
            j3s.Add(new Ubaj3sModule());
            ht.Add(new UbaHtModule("currency_uridium", j3s));

            var l4b = new List<Ubal4bModule>();
            l4b.Add(new Ubal4bModule("sezon", 1));

            player.SendCommand(UbaCommand.write(new UbaG3FModule(1, 1, 1, 1552), new Uba64iModule("label_traininggrounds_season", 120000, ht), new UbahsModule(l4b)));

            //player.SendCommand(command_z3Q.write(command_z3Q.varC2f));
            /*
            var contacts = new List<class_i1d>();

            var DDDDD = new List<class_84I>();
            

            var DDDDsD = new List<class_533>();
            DDDDsD.Add(new class_533(class_533.varN2g));

            contacts.Add(new class_i1d(new class_O4f(2, DDDDD), new class_y3i(2, DDDDsD)));
            contacts.Add(new class_i1d(new class_O4f(1, DDDDD), new class_y3i(1, DDDDsD)));

            player.SendCommand(ContactsListUpdateCommand.write(new class_o3q(contacts), new class_g1a(true, true, true, true), new class_H4Q(false)));
            */



            player.SendCommand(PetInitializationCommand.write(true, true, true));
            player.CheckAbilities(player);
            /*
            var ht = new List<UbaHtModule>();
            var j3s = new List<Ubaj3sModule>();
            j3s.Add(new Ubaj3sModule());
            ht.Add(new UbaHtModule("test", j3s));

            var l4b = new List<Ubal4bModule>();
            l4b.Add(new Ubal4bModule("test", 1));

            player.SendCommand(UbaCommand.write(new UbaRankModule(1,1,1,1), new Uba64iModule("test",1, ht), new UbahsModule(l4b)));
            */


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

        public static List<int> ShapeCordsToInts(List<Position> ShapeCords)
        {
            List<int> cords = new List<int>();
            foreach (var cord in ShapeCords)
            {
                cords.Add(cord.X);
                cords.Add(cord.Y);
            }
            return cords;
        }

        public static void SendSettings(Player player, bool isLogin = true)
        {
            player.SettingsManager.SendUserKeyBindingsUpdateCommand();
            if (isLogin)
            {
                player.SettingsManager.SendSlotItemsBars();
            }
            player.SettingsManager.SendUserSettingsCommand();
            player.SettingsManager.SendMenuBarsCommand();
            player.SettingsManager.SendSlotBarCommand();
            player.SettingsManager.SendHelpWindows();
            player.SetCurrentCooldowns();
            player.SendCurrentCooldowns();
        }
    }
}
