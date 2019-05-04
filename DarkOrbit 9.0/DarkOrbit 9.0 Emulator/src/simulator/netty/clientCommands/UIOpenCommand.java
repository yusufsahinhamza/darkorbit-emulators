package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the logout request

 @author Ordepsousa
 @date 22/07/2014
 @file UIOpenCommand.java
 @package game.objects.netty.commands */
public class UIOpenCommand
        extends ClientCommand {

    public static final int ID = 29742;

    private String mItemId;

    // this action is sent when user opens user stats window
    public static final String ACTION_USER = "user";

    // this action is sent when user opens ship stats window
    public static final String ACTION_SHIP = "ship";

    // this action is sent when user opens ship warp window ?????
    public static final String ACTION_SHIP_WARP = "ship_warp";

    // this action is sent when user opens chat window
    public static final String ACTION_CHAT = "chat";

    // this action is sent when user opens group window
    public static final String ACTION_GROUP = "group";

    // this action is sent when user opens minimap window
    public static final String ACTION_MINIMAP = "minimap";

    // this action is sent when user opens spacemap window
    public static final String ACTION_SPACEMAP = "spacemap";

    // this action is sent when user opens missions window
    public static final String ACTION_QUESTS = "quests";

    // this action is sent when user opens refinement window
    public static final String ACTION_REFINEMENT = "refinement";

    // this action is sent when user opens log window
    public static final String ACTION_LOG = "log";

    // this action is sent when user opens pet window
    public static final String ACTION_PET = "pet";

    // this action is sent when user opens contacts window
    public static final String ACTION_CONTACTS = "contacts";

    // this action is sent when user opens logout window
    public static final String ACTION_LOGOUT = "logout";

    /**
     Constructor
     */
    public UIOpenCommand(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            this.mItemId = in.readUTF();
            in.readShort();
            in.readShort();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public String getAction() {
        return this.mItemId;
    }

}