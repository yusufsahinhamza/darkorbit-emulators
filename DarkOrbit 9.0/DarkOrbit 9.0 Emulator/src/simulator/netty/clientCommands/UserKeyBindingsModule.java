package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;
import java.util.Vector;

/**
 Description: This is the user key bindings module

 @author Asus
 @date 23/07/2014
 @file UserKeyBindingsModule.java
 @package game.objects.netty.commands */
public class UserKeyBindingsModule {

    public DataInputStream in;
    public static int id = 23100;
    public final Vector<Integer> keyCodes;
    public static final short JUMP                   = 0;
    public static final short CHANGE_CONFIG          = 1;
    public static final short ACTIVATE_LASER         = 2;
    public static final short LAUNCH_ROCKET          = 3;
    public static final short PET_ACTIVATE           = 4;
    public static final short PET_GUARD_MODE         = 5;
    public static final short LOGOUT                 = 6;
    public static final short QUICKSLOT              = 7;
    public static final short QUICKSLOT_PREMIUM      = 8;
    public static final short TOGGLE_WINDOWS         = 9;
    public static final short PERFORMANCE_MONITORING = 10;
    public static final short ZOOM_IN                = 11;
    public static final short ZOOM_OUT               = 12;
    public static final short PET_REPAIR_SHIP        = 13;
    public short actionType;
    public int   parameter;
    public short charCode;

    /**
     Constructor
     */
    public UserKeyBindingsModule(DataInputStream in) {
        this.in = in;
        keyCodes = new Vector<Integer>();
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            this.parameter = in.readInt();
            this.parameter = this.parameter << 11 | this.parameter >>> 21;
            in.readShort();
            int i = 0;
            int length = in.readInt();
            while (i < length) {
                int keyCode = in.readInt();
                keyCode = (keyCode << 9 | keyCode >>> 23);
                keyCodes.add(keyCode);
                i++;
            }
            actionType = in.readShort();
            in.readShort();
            charCode = in.readShort();
        } catch (IOException e) {
        }
    }
}
