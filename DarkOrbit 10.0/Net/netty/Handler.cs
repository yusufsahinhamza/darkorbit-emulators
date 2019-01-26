using Ow.Game;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.handlers;
using Ow.Net.netty.handlers.GroupRequestHandlers;
using Ow.Net.netty.handlers.UbaRequestHandlers;
using Ow.Net.netty.requests;
using Ow.Net.netty.requests.GroupRequests;
using Ow.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Net.netty
{
    class Handler
    {
        private static Dictionary<short, IHandler> Commands = new Dictionary<short, IHandler>();

        public static void AddCommands()
        {
            Commands.Add(LegacyModuleRequest.ID, new LegacyModuleHandler());
            Commands.Add(MoveRequest.ID, new MoveRequestHandler());
            Commands.Add(ShipSelectRequest.ID, new ShipSelectRequestHandler());
            Commands.Add(SelectMenuBarItemRequest.ID, new SelectMenuBarItemHandler());
         //   Commands.Add(ReadyRequest.ID, new ReadyRequestHandler());
            Commands.Add(UIOpenRequest.ID, new UIOpenRequestHandler());
            Commands.Add(SlotBarConfigSetRequest.ID, new SlotBarConfigSetRequestHandler());
            Commands.Add(AudioSettingsRequest.ID, new AudioSettingsRequestHandler());
            Commands.Add(DisplaySettingsRequest.ID, new DisplaySettingsRequestHandler());
            Commands.Add(GameplaySettingsRequest.ID, new GameplaySettingsRequestHandler());
            Commands.Add(QualitySettingsRequest.ID, new QualitySettingsRequestHandler());
            Commands.Add(WindowSettingsRequest.ID, new WindowSettingsRequestHandler());
            Commands.Add(UserKeyBindingsUpdateRequest.ID, new UserKeyBindingsUpdateHandler());
            Commands.Add(AssetHandleClickRequest.ID, new AssetHandleClickHandler());
            Commands.Add(KillscreenRequest.ID, new KillsceenRequestHandler());
            Commands.Add(ProActionBarRequest.ID, new ProActionBarRequestHandler());
            Commands.Add(PetRequest.ID, new PetRequestHandler());
            Commands.Add(EquipModuleRequest.ID, new EquipModuleRequestHandler());
            Commands.Add(GroupInvitationRequest.ID, new GroupInvitationRequestHandler());
            Commands.Add(GroupAcceptInvitationRequest.ID, new GroupAcceptInvitationRequestHandler());
            //   Commands.Add(WindowSettingsRequest.ID, new WindowSettingsRequestHandler());
            Commands.Add(CollectBoxRequest.ID, new CollectBoxRequestHandler());
            Commands.Add(PetGearActivationRequest.ID, new PetGearActivationRequestHandler());
            Commands.Add(2244, new RepairStationRequestHandler());
            Commands.Add(10343, new LogoutCancelRequestHandler());
            Commands.Add(31106, new AttackLaserRequestHandler());
            Commands.Add(1528, new AttackAbortLaserRequestHandler());
            Commands.Add(22801, new AttackRocketRequestHandler());
            Commands.Add(9253, new PortalJumpRequestHandler());
            Commands.Add(11301, new UbaMatchmakingRequestHandler());
            Commands.Add(659, new UbaMatchmakingCancelRequestHandler());
            Commands.Add(65, new UbaMatchmakingAcceptRequestHandler());
            Commands.Add(9103, new GroupLeaveRequestHandler());
            Commands.Add(28685, new GroupChangeGroupBehaviourRequestHandler());
            Commands.Add(26571, new GroupUpdateBlockInvitationStateRequestHandler());
            Commands.Add(GroupRevokeInvitationRequest.ID, new GroupRevokeInvitationRequestHandler());
            Commands.Add(GroupRejectInvitationRequest.ID, new GroupRejectInvitationRequestHandler());
            Commands.Add(GroupChangeLeaderRequest.ID, new GroupChangeLeaderRequestHandler());
            Commands.Add(GroupPingPlayerRequest.ID, new GroupPingPlayerRequestHandler());
            Commands.Add(GroupPingPositionRequest.ID, new GroupPingPositionRequestHandler());
            Commands.Add(GroupFollowPlayerRequest.ID, new GroupFollowPlayerRequestHandler());
            Commands.Add(GroupKickPlayerRequest.ID, new GroupKickPlayerRequestHandler());
        }

        public static void Execute(byte[] bytes, GameClient client)
        {
            var parser = new ByteParser(bytes);

            /*
             * açılacaksa GameClient.cs'deki Send fonksiyonundaki kontroller kaldırılmalıdır
             * 
             * 
             * 
            if (parser.CMD_ID == 666)
            {
                
                //last
                client.Send(ShipInitializationCommandLast.write(1, "LEJYONER", "ship_cyborg_design_cyborg-carbonite", 540, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 3, true, 0, 0, 0, 0, 0, 0, 21, "", 100, true, false, true, new List<VisualModifierCommandLast>()));
                client.Send(ShipCreateCommandLast.write(2, "ship_goliath", 3, "", "TEST", 0, 0, 1, 0, 20, false, new ClanRelationModuleLast(ClanRelationModuleLast.NON_AGGRESSION_PACT), 100, true, false, false, 0, 2, "", new List<VisualModifierCommandLast>(), new class_q1OLast(class_q1OLast.DEFAULT)));
                //last


                //8.4.5
                client.Send(ShipInitializationCommand845.write(1, "LEJYONER", "ship_goliath_design_bastion", 540, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 3, true, 0 ,0 ,0 ,0,0,0,20,"",100,true,false,true,new List<VisualModifierCommand845>()));
                var DronePacket = "2|6|2|2|6|2|2|6|2|2|6|2|2|6|2|2|6|2|2|6|2|2|6|2|2|6|2|2|6|2|3|6|2|4|6|2";
                var drones = "0|n|d|" + 1 + "|" + 0 + "|" + DronePacket;
                client.Send(LegacyModule845.write("0|n|t|" +1 + "|366|" + "jackpot_arena_winner"));
                client.Send(LegacyModule845.write(drones));
                //8.4.5

            }
            */

            if (parser.CMD_ID == LoginRequest.ID)
            {
                var read = new LoginRequest();
                read.readCommand(bytes);
                new LoginRequestHandler(client, read.userID);
                return;
            }

            var gameSession = GameManager.GetGameSession(client.UserId);
            if (gameSession == null) return;

            if (Commands.ContainsKey(parser.CMD_ID))
            {
                Commands[parser.CMD_ID].execute(gameSession, bytes);
                gameSession.LastActiveTime = DateTime.Now;
            }
            else
                Out.WriteLine("Unknown command ID: " + parser.CMD_ID);
        }
    }
}
