package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 08-04-2015.
 */
public class class_829 {

    public static int ID = 31436;

    public int     var_2454 = 0;
    public boolean name_61  = false;

    public class_829(boolean param1, int param2) {
        this.name_61 = param1;
        this.var_2454 = param2;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.var_2454 >>> 15 | this.var_2454 << 17);
            param1.writeBoolean(this.name_61);
            param1.writeShort(-13647);
        } catch (IOException e) {
        }
    }
}
