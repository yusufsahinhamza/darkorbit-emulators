package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 11-03-2015.
 */
public class WindowIDModule {

    public WindowIDModule(short param1) {
        this.var_2328 = param1;
    }

    public static short short_2067     = 0;
    public static short PET            = 19;
    public static short short_1617     = 11;
    public static short short_39       = 21;
    public static short short_448      = 26;
    public static short short_2149     = 2;
    public static short short_1903     = 8;
    public static short short_1723     = 25;
    public static short short_785      = 12;
    public static short short_1530     = 10;
    public static short ACHIEVEMENTS   = 18;
    public static short CLI            = 15;
    public static short HELP           = 6;
    public static short TDM            = 13;
    public static short short_2401     = 1;
    public static short short_102      = 17;
    public static short SETTINGS       = 5;
    public static short short_2406     = 14;
    public static short short_1894     = 24;
    public static short short_2694     = 4;
    public static short short_1556     = 16;
    public static short VIDEO_WINDOW   = 20;
    public static short short_2122     = 7;
    public static short LOG            = 3;
    public static short short_1610     = 23;
    public static short short_2359     = 9;
    public static short MINIMAP_WINDOW = 22;

    public static int ID = 9298;

    public short var_2328 = 0;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(this.var_2328);
        } catch (IOException e) {
        }
    }
}
