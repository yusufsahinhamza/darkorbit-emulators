package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 17-03-2015.
 */
public class class_873 {

    public class_873(short param1) {
        this.var_1950 = param1;
    }

    public static short const_1073 = 4;
    public static short const_2245 = 0;
    public static short const_1766 = 2;
    public static short const_281  = 1;
    public static short const_517  = 3;
    public static short const_2601 = 5;

    public static int ID = 6656;

    public short var_1950;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(1731);
            param1.writeShort(this.var_1950);
            param1.writeShort(8764);
        } catch (IOException e) {
        }
    }
}
