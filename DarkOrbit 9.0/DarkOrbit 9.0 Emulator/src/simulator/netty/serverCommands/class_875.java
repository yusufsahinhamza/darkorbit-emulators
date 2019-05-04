package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 17-03-2015.
 */
public class class_875 {

    public static int ID = 4884;

    public boolean var_1316 = false;

    public class_875(boolean param1) {
        this.var_1316 = param1;
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
            param1.writeShort(-8392);
            param1.writeShort(-13307);
            param1.writeBoolean(this.var_1316);
        } catch (IOException e) {
        }
    }
}
