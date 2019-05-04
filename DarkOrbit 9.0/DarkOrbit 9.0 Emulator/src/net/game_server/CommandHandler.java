package net.game_server;

import java.util.HashMap;
import simulator.netty.ClientCommand;
import simulator.netty.clientCommands.AssetHandleClickRequest;
import simulator.netty.clientCommands.AttackAbortLaserRequest;
import simulator.netty.clientCommands.AttackLaserRequest;
import simulator.netty.clientCommands.AttackRocketRequest;
import simulator.netty.clientCommands.AudioSettingsRequest;
import simulator.netty.clientCommands.CollectBoxRequest;
import simulator.netty.clientCommands.DisplaySettingsRequest;
import simulator.netty.clientCommands.GameplaySettingsRequest;
import simulator.netty.clientCommands.GroupSendRequest;
import simulator.netty.clientCommands.KillScreenRepairButtonRequest;
import simulator.netty.clientCommands.LabRefinementRequest;
import simulator.netty.clientCommands.LabUpdateItemRequest;
import simulator.netty.clientCommands.LabUpdateRequest;
import simulator.netty.clientCommands.LegacyModuleRequest;
import simulator.netty.clientCommands.LogoutCancelRequest;
import simulator.netty.clientCommands.MoveRequest;
import simulator.netty.clientCommands.PetGearActivationRequest;
import simulator.netty.clientCommands.PetRequest;
import simulator.netty.clientCommands.PortalJumpRequest;
import simulator.netty.clientCommands.QualitySettingsRequest;
import simulator.netty.clientCommands.ReadyRequest;
import simulator.netty.clientCommands.RepairStationRequest;
import simulator.netty.clientCommands.SelectMenuBarItem;
import simulator.netty.clientCommands.ShipSelectRequest;
import simulator.netty.clientCommands.ShipWarpRequest;
import simulator.netty.clientCommands.SlotBarConfigSetRequest;
import simulator.netty.clientCommands.UIOpenCommand;
import simulator.netty.clientCommands.UserKeyBindingsUpdate;
import simulator.netty.clientCommands.VersionRequest;
import simulator.netty.clientCommands.WindowSettingsRequest;
import simulator.netty.handlers.AssetHandleClickHandler;
import simulator.netty.handlers.AttackAbortLaserRequestHandler;
import simulator.netty.handlers.AttackLaserRequestHandler;
import simulator.netty.handlers.AttackRocketRequestHandler;
import simulator.netty.handlers.AudioSettingsRequestHandler;
import simulator.netty.handlers.CollectBoxRequestHandler;
import simulator.netty.handlers.DisplaySettingsRequestHandler;
import simulator.netty.handlers.GameplaySettingsRequestHandler;
import simulator.netty.handlers.GroupSendRequestHandler;
import simulator.netty.handlers.ICommandHandler;
import simulator.netty.handlers.KillScreenRepairButtonRequestHandler;
import simulator.netty.handlers.LabRefinementRequestHandler;
import simulator.netty.handlers.LabUpdateItemRequestHandler;
import simulator.netty.handlers.LabUpdateRequestHandler;
import simulator.netty.handlers.LegacyModuleHandler;
import simulator.netty.handlers.LogoutCancelRequestHandler;
import simulator.netty.handlers.MoveRequestHandler;
import simulator.netty.handlers.PetGearActivationRequestHandler;
import simulator.netty.handlers.PetRequestHandler;
import simulator.netty.handlers.PortalJumpRequestHandler;
import simulator.netty.handlers.QualitySettingsRequestHandler;
import simulator.netty.handlers.ReadyRequestHandler;
import simulator.netty.handlers.RepairStationRequestHandler;
import simulator.netty.handlers.SelectMenuBarItemHandler;
import simulator.netty.handlers.ShipSelectRequestHandler;
import simulator.netty.handlers.ShipWarpRequestHandler;
import simulator.netty.handlers.SlotBarConfigSetRequestHandler;
import simulator.netty.handlers.UIOpenCommandHandler;
import simulator.netty.handlers.UserKeyBindingsUpdateHandler;
import simulator.netty.handlers.VersionRequestHandler;
import simulator.netty.handlers.WindowSettingsRequestHandler;
import utils.Settings;
import utils.Log;

/**
 Created by Pedro on 30-03-2015.
 */
public class CommandHandler {

    private static HashMap<Integer, Class> mHandlers;

    public static void initHandler() {
        mHandlers = new HashMap<>();
        mHandlers.put(VersionRequest.ID, VersionRequestHandler.class);
        mHandlers.put(ReadyRequest.ID, ReadyRequestHandler.class);
        mHandlers.put(MoveRequest.ID, MoveRequestHandler.class);
        mHandlers.put(UIOpenCommand.ID, UIOpenCommandHandler.class);
        mHandlers.put(LogoutCancelRequest.ID, LogoutCancelRequestHandler.class);
        mHandlers.put(PortalJumpRequest.ID, PortalJumpRequestHandler.class);
        mHandlers.put(AudioSettingsRequest.ID, AudioSettingsRequestHandler.class);
        mHandlers.put(DisplaySettingsRequest.ID, DisplaySettingsRequestHandler.class);
        mHandlers.put(QualitySettingsRequest.ID, QualitySettingsRequestHandler.class);
        mHandlers.put(GameplaySettingsRequest.ID, GameplaySettingsRequestHandler.class);
        mHandlers.put(UserKeyBindingsUpdate.ID, UserKeyBindingsUpdateHandler.class);
        mHandlers.put(ShipSelectRequest.ID, ShipSelectRequestHandler.class);
        mHandlers.put(AttackLaserRequest.ID, AttackLaserRequestHandler.class);
        mHandlers.put(AttackAbortLaserRequest.ID, AttackAbortLaserRequestHandler.class);
        mHandlers.put(SelectMenuBarItem.ID, SelectMenuBarItemHandler.class);
        mHandlers.put(SlotBarConfigSetRequest.ID, SlotBarConfigSetRequestHandler.class);
        mHandlers.put(AttackRocketRequest.ID, AttackRocketRequestHandler.class);
        mHandlers.put(LegacyModuleRequest.ID, LegacyModuleHandler.class);
        mHandlers.put(AssetHandleClickRequest.ID, AssetHandleClickHandler.class);
        mHandlers.put(LabUpdateRequest.ID, LabUpdateRequestHandler.class);
        mHandlers.put(LabRefinementRequest.ID, LabRefinementRequestHandler.class);
        mHandlers.put(LabUpdateItemRequest.ID, LabUpdateItemRequestHandler.class);
        mHandlers.put(CollectBoxRequest.ID, CollectBoxRequestHandler.class);
        mHandlers.put(RepairStationRequest.ID, RepairStationRequestHandler.class);
        mHandlers.put(PetRequest.ID, PetRequestHandler.class);
        mHandlers.put(PetGearActivationRequest.ID, PetGearActivationRequestHandler.class);
        mHandlers.put(KillScreenRepairButtonRequest.ID, KillScreenRepairButtonRequestHandler.class);
        mHandlers.put(GroupSendRequest.ID, GroupSendRequestHandler.class);
        mHandlers.put(ShipWarpRequest.ID, ShipWarpRequestHandler.class);
        mHandlers.put(WindowSettingsRequest.ID, WindowSettingsRequestHandler.class);
    }

    @SuppressWarnings("unchecked")
    public static ICommandHandler getCommandHandler(final GameServerClientConnection pGameServerClientConnection,
                                                    final ClientCommand pClientCommand) {
        try {
        	if(pClientCommand != null) {
            final Class commandHandlerClass = mHandlers.get(pClientCommand.getID());
            if (commandHandlerClass != null) {
            	if(Settings.TEXTS_ENABLED) { Log.p("returned ICommandHandler with ID = " + pClientCommand.getID()); }
                return (ICommandHandler) commandHandlerClass.getConstructor(GameServerClientConnection.class,
                                                                            ClientCommand.class)
                                                            .newInstance(pGameServerClientConnection, pClientCommand);
            } else {
                Log.p("Couldn't get command handler for ID = " + pClientCommand.getID());
            }
        	}
        } catch (Exception e) {
            Log.p("Something went wrong getting command handler at CommandHandler");
            e.printStackTrace();
        }
        return null;
    }
}
