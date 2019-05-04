package net.game_server;

import java.io.DataInputStream;
import java.util.HashMap;

import simulator.netty.ClientCommand;
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
import simulator.netty.clientCommands.PortalJumpRequest;
import simulator.netty.clientCommands.AssetHandleClickRequest;
import simulator.netty.clientCommands.WindowSettingsRequest;
import utils.Settings;
import utils.Log;

/**
 Created by Pedro on 30-03-2015.
 */
public class CommandLookup {

    private static HashMap<Integer, Class> mCommandsLookup;

    public static void initLookup() {
        mCommandsLookup = new HashMap<>();
        mCommandsLookup.put(VersionRequest.ID, VersionRequest.class);
        mCommandsLookup.put(ReadyRequest.ID, ReadyRequest.class);
        mCommandsLookup.put(MoveRequest.ID, MoveRequest.class);
        mCommandsLookup.put(UIOpenCommand.ID, UIOpenCommand.class);
        mCommandsLookup.put(LogoutCancelRequest.ID, LogoutCancelRequest.class);
        mCommandsLookup.put(PortalJumpRequest.ID, PortalJumpRequest.class);
        mCommandsLookup.put(AudioSettingsRequest.ID, AudioSettingsRequest.class);
        mCommandsLookup.put(DisplaySettingsRequest.ID, DisplaySettingsRequest.class);
        mCommandsLookup.put(QualitySettingsRequest.ID, QualitySettingsRequest.class);
        mCommandsLookup.put(GameplaySettingsRequest.ID, GameplaySettingsRequest.class);
        mCommandsLookup.put(UserKeyBindingsUpdate.ID, UserKeyBindingsUpdate.class);
        mCommandsLookup.put(ShipSelectRequest.ID, ShipSelectRequest.class);
        mCommandsLookup.put(AttackLaserRequest.ID, AttackLaserRequest.class);
        mCommandsLookup.put(AttackAbortLaserRequest.ID, AttackAbortLaserRequest.class);
        mCommandsLookup.put(SelectMenuBarItem.ID, SelectMenuBarItem.class);
        mCommandsLookup.put(SlotBarConfigSetRequest.ID, SlotBarConfigSetRequest.class);
        mCommandsLookup.put(AttackRocketRequest.ID, AttackRocketRequest.class);
        mCommandsLookup.put(LegacyModuleRequest.ID, LegacyModuleRequest.class);
        mCommandsLookup.put(AssetHandleClickRequest.ID, AssetHandleClickRequest.class);
        mCommandsLookup.put(LabUpdateRequest.ID, LabUpdateRequest.class);
        mCommandsLookup.put(LabRefinementRequest.ID, LabRefinementRequest.class);
        mCommandsLookup.put(LabUpdateItemRequest.ID, LabUpdateItemRequest.class);
        mCommandsLookup.put(CollectBoxRequest.ID, CollectBoxRequest.class);
        mCommandsLookup.put(RepairStationRequest.ID, RepairStationRequest.class);
        mCommandsLookup.put(PetRequest.ID, PetRequest.class);
        mCommandsLookup.put(PetGearActivationRequest.ID, PetGearActivationRequest.class);
        mCommandsLookup.put(KillScreenRepairButtonRequest.ID, KillScreenRepairButtonRequest.class);
        mCommandsLookup.put(GroupSendRequest.ID, GroupSendRequest.class);
        mCommandsLookup.put(ShipWarpRequest.ID, ShipWarpRequest.class);
        mCommandsLookup.put(WindowSettingsRequest.ID, WindowSettingsRequest.class);
    }

    @SuppressWarnings("unchecked")
    public static ClientCommand getCommand(final DataInputStream pDataInputStream) {
        try {
        	if(pDataInputStream != null) {
            final int commandID = pDataInputStream.readShort();
            final Class commandClass = mCommandsLookup.get(commandID);
            if (commandClass != null) {
                final ClientCommand clientCommand =
                        (ClientCommand) commandClass.getDeclaredConstructor(DataInputStream.class)
                                                    .newInstance(pDataInputStream);
                clientCommand.read();
                return clientCommand;
            } else {
            	if(Settings.TEXTS_ENABLED) { Log.p("Couldn't get command with ID = " + commandID); }
            }
        	}
        } catch (Exception e) {
     //       Log.p("Something went wrong getting command at CommandLookup");
     //       e.printStackTrace();
        }
        return null;
    }
}
