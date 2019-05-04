package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 08-04-2015.
 */
public class class_835 {

    public static int   ID            = 28613;
    public static short FEATURE_MENU  = 1;
    public static short ITEMS_CONTROL = 0;
    public static short WINDOW        = 2;

    public short var_3212 = 0;

    public class_835(short param1) {
        this.var_3212 = param1;
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
            param1.writeShort(19751);
            param1.writeShort(this.var_3212);
        } catch (IOException e) {
        }
    }
}
