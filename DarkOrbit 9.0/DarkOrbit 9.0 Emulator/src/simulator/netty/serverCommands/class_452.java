package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 08-04-2015.
 */
public class class_452 {

    public static int   ID          = 5112;
    public static short RED_BLINK   = 5;
    public static short ACTIVE      = 6;
    public static short BLUE_BLINK2 = 1;
    public static short ARROWS      = 2;
    public static short BLUE_BLINK3 = 3;
    public static short BLUE_BLINK4 = 4;
    public static short BLUE_BLINK  = 0;

    public short type = 0;

    public class_452(short param1) {
        this.type = param1;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(this.type);
        } catch (IOException e) {
        }
    }
}
