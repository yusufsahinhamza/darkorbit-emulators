using Ow.Game;
using Ow.Game.Events;
using Ow.Game.Movements;
using Ow.Game.Objects;
using Ow.Game.Objects.Players.Managers;
using Ow.Managers;
using Ow.Managers.MySQLManager;
using Ow.Net.netty.commands;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty.handlers
{
    class LoginRequestHandler
    {
        public Player Player { get; set; }
        public GameSession GameSession { get; set; }

        public LoginRequestHandler(GameClient client, int userId)
        {
            try
            {
                client.UserId = userId;

                var gameSession = GameManager.GetGameSession(userId);
                if (gameSession != null)
                {
                    Player = gameSession.Player;
                    gameSession.Disconnect(GameSession.DisconnectionType.NORMAL);
                }
                else Player = QueryManager.GetPlayer(userId);

                if (Player != null)
                {
                    GameSession = new GameSession(Player)
                    {
                        Client = client,
                        LastActiveTime = DateTime.Now
                    };
                }
                else
                {
                    Out.WriteLine("Failed loading user ship / ShipInitializationHandler ERROR");
                    return;
                }

                Execute();

                if (Player.Destroyed) Player.KillScreen(null, DestructionType.MISC, true);
                else
                {
                    SendSettings(Player);
                    SendPlayer(Player);
                    Player.Spacemap.AddCharacter(Player);
                }
            }
            catch (Exception e)
            {
                Out.WriteLine("UID: " + Player.Id + " Exception: " + e, "LoginRequestHandler.cs");
                Logger.Log("error_log", $"- [LoginRequestHandler.cs] LoginRequestHandler class exception: {e}");
            }
        }

        public void Execute()
        {
            try
            {
                if (GameSession == null) return;

                if (!GameManager.GameSessions.ContainsKey(Player.Id))
                    GameManager.GameSessions.TryAdd(Player.Id, GameSession);
                else
                {
                    GameManager.GameSessions[Player.Id].Disconnect(GameSession.DisconnectionType.NORMAL);
                    GameManager.GameSessions[Player.Id] = GameSession;
                }

                if (Player.Spacemap == null || Player.Position == null || GameManager.GetSpacemap(Player.Spacemap.Id) == null)
                {
                    Player.CurrentHitPoints = Player.MaxHitPoints;
                    Player.CurrentShieldConfig1 = Player.MaxShieldPoints;
                    Player.CurrentShieldConfig2 = Player.MaxShieldPoints;

                    if (Player.RankId == 21)
                        Player.Premium = true;

                    Player.Spacemap = GameManager.GetSpacemap(Player.GetBaseMapId());
                    Player.SetPosition(Player.GetBasePosition());
                }

                Program.TickManager.AddTick(Player);

                QueryManager.SavePlayer.Information(Player);
                Console.Title = $"DarkOrbit | {GameManager.GameSessions.Count} users online";
            }
            catch (Exception e)
            {
                Out.WriteLine("UID: "+Player.Id+" Execute void exception: " + e, "LoginRequestHandler.cs");
                Logger.Log("error_log", $"- [LoginRequestHandler.cs] Execute void exception: {e}");
            }
        }

        public static void SendPlayer(Player player)
        {
            try
            {
                player.SendCommand(player.GetShipInitializationCommand());

                if (player.Title != "")
                    player.SendPacket($"0|n|t|{player.Id}|1|{player.Title}");

                player.SendPacket(player.DroneManager.GetDronesPacket());
                player.SendCommand(DroneFormationChangeCommand.write(player.Id, DroneManager.GetSelectedFormationId(player.Settings.InGameSettings.selectedFormation)));
                player.SendPacket("0|S|CFG|" + player.CurrentConfig);
          
                player.SendPacket($"0|A|BK|{player.Equipment.Items.BootyKeys}");
                //player.SendPacket("0|A|JV|0"); //atlama kuponu miktarı

                if (player.Group != null)
                    player.Group.InitializeGroup(player);
                
                var spaceball = EventManager.Spaceball.Character;
                if (EventManager.Spaceball.Active && spaceball != null)
                    player.SendPacket($"0|n|ssi|{spaceball.Mmo}|{spaceball.Eic}|{spaceball.Vru}");
                else
                    player.SendPacket($"0|n|ssi|0|0|0");
                /*
                var priceList = new List<JumpCPUPriceMappingModule>();
                var price = new List<int>();
                price.Add(1);
                price.Add(14);
                price.Add(15);
                price.Add(16);
                priceList.Add(new JumpCPUPriceMappingModule(0, price));
                player.SendCommand(JumpCPUUpdateCommand.write(priceList));

                player.SendCommand(CpuInitializationCommand.write(true, false));
                */
                //player.SendPacket("0|A|JCPU|S|20|1"); //ATLAMA GERİ SAYIM

                player.SendCommand(SetSpeedCommand.write(player.Speed, player.Speed));
                player.Spacemap.SendObjects(player);

                player.BoosterManager.Update();

                if (player.Pet != null)
                {
                    player.SendCommand(PetInitializationCommand.write(true, true, !player.Settings.InGameSettings.petDestroyed));

                    if (player.Settings.InGameSettings.petDestroyed)
                        player.SendCommand(PetUIRepairButtonCommand.write(true, 250));
                }
                
                player.UpdateStatus();

                //player.SendCommand(UbaWindowInitializationCommand.write(new Ubas3wModule(new UbaG3FModule(0, 0, 0, 0), new Uba64iModule("", 0, new //List<UbaHtModule>()), new UbahsModule(new List<Ubal4bModule>())), 0));


                //player.SendCommand(VideoWindowCreateCommand.write(1, "l", true, new List<string> { "start_head", "tutorial_video_msg_reward_premium_intro" }, 1, 1));    

                //player.SendPacket("0|n|KSMSG|start_head");


                //player.SendCommand(command_e4W.write(1, "", new class_oS(0, 0, 0, 0, 0, 0, 0, 0, 0, 0), "", new class_s16(1, class_s16.varC3p), 1));

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

                /*
                if (isLogin)
                {
                UBA SEASON

                    var ht = new List<UbaHtModule>();
                    var j3s = new List<command_j3s>();
                    j3s.Add(new Ubaf3kModule("currency_uridium", 250000));
                    ht.Add(new UbaHtModule("asddas", j3s));

                    var l4b = new List<Ubal4bModule>();
                    l4b.Add(new Ubal4bModule("Bilmemne Sezonu", 2));

                    player.SendCommand(UbaWindowInitializationCommand.write(new Ubas3wModule(new UbaG3FModule(55, 60, 5, 333443), new Uba64iModule("Yaz Sezonu", 1, ht), new UbahsModule(l4b)), 0));
                }
                */

                player.SendCommand(player.GetBeaconCommand());
            }
            catch (Exception e)
            {
                Out.WriteLine("UID: " + player.Id + " SendPlayerItems void exception: " + e, "LoginRequestHandler.cs");
                Logger.Log("error_log", $"- [LoginRequestHandler.cs] SendPlayerItems void exception: {e}");
            }
        }

        public static void SendSettings(Player player)
        {
            try
            {
                player.SetCurrentCooldowns();
                player.SettingsManager.SendUserKeyBindingsUpdateCommand();
                player.SettingsManager.SendUserSettingsCommand();
                player.SettingsManager.SendMenuBarsCommand();
                player.SettingsManager.SendSlotBarCommand();
                player.SettingsManager.SendHelpWindows();
            }
            catch (Exception e)
            {
                Out.WriteLine("UID: " + player.Id + " SendSettings void exception: " + e, "LoginRequestHandler.cs");
                Logger.Log("error_log", $"- [LoginRequestHandler.cs] SendSettings void exception: {e}");
            }
        }
    }
}
