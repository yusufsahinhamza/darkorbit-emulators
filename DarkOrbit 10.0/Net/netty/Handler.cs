using Ow.Game;
using Ow.Managers;
using Ow.Net.netty.commands;
using Ow.Net.netty.handlers;
using Ow.Net.netty.handlers.BattleStationRequestHandlers;
using Ow.Net.netty.handlers.GroupRequestHandlers;
using Ow.Net.netty.handlers.PetRequestHandlers;
using Ow.Net.netty.handlers.UbaRequestHandlers;
using Ow.Net.netty.requests;
using Ow.Net.netty.requests.BattleStationRequests;
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
            Commands.Add(SendWindowUpdateRequest.ID, new SendWindowUpdateRequestHandler());
            Commands.Add(WindowSettingsRequest.ID, new WindowSettingsRequestHandler());
            Commands.Add(UserKeyBindingsUpdateRequest.ID, new UserKeyBindingsUpdateHandler());
            Commands.Add(AssetHandleClickRequest.ID, new AssetHandleClickHandler());
            Commands.Add(KillscreenRequest.ID, new KillsceenRequestHandler());
            Commands.Add(ProActionBarRequest.ID, new ProActionBarRequestHandler());
            Commands.Add(PetRequest.ID, new PetRequestHandler());
            Commands.Add(GroupInvitationRequest.ID, new GroupInvitationRequestHandler());
            Commands.Add(GroupAcceptInvitationRequest.ID, new GroupAcceptInvitationRequestHandler());
            Commands.Add(CollectBoxRequest.ID, new CollectBoxRequestHandler());
            Commands.Add(PetGearActivationRequest.ID, new PetGearActivationRequestHandler());
            Commands.Add(GroupRevokeInvitationRequest.ID, new GroupRevokeInvitationRequestHandler());
            Commands.Add(GroupRejectInvitationRequest.ID, new GroupRejectInvitationRequestHandler());
            Commands.Add(GroupChangeLeaderRequest.ID, new GroupChangeLeaderRequestHandler());
            Commands.Add(GroupPingPlayerRequest.ID, new GroupPingPlayerRequestHandler());
            Commands.Add(GroupPingPositionRequest.ID, new GroupPingPositionRequestHandler());
            Commands.Add(GroupFollowPlayerRequest.ID, new GroupFollowPlayerRequestHandler());
            Commands.Add(GroupKickPlayerRequest.ID, new GroupKickPlayerRequestHandler());

            Commands.Add(EquipModuleRequest.ID, new EquipModuleRequestHandler());
            Commands.Add(BuildStationRequest.ID, new BuildStationRequestHandler());
            Commands.Add(UnEquipModuleRequest.ID, new UnEquipModuleRequestHandler());
            Commands.Add(EmergencyRepairRequest.ID, new EmergencyRepairRequestHandler());

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
        }

        public static void Execute(byte[] bytes, GameClient client)
        {
            try
            {
                var parser = new ByteParser(bytes);

                if (parser.ID == LoginRequest.ID)
                {
                    var read = new LoginRequest();
                    read.readCommand(bytes);

                    if (QueryManager.CheckSessionId(read.userID, read.sessionID) && !QueryManager.Banned(read.userID))
                        new LoginRequestHandler(client, read.userID);

                    return;
                }

                var gameSession = GameManager.GetGameSession(client.UserId);
                if (gameSession == null) return;

                if (Commands.ContainsKey(parser.ID))
                {
                    Commands[parser.ID].execute(gameSession, bytes);
                    gameSession.LastActiveTime = DateTime.Now;
                }
                //else Out.WriteLine("Unknown command ID: " + parser.ID);
            }
            catch (Exception e)
            {
                Out.WriteLine("Execute void exception: " + e, "Handler.cs");
                Logger.Log("error_log", $"- [Handler.cs] Execute void exception: {e}");
            }
        }
    }
}
