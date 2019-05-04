package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 14-03-2015.
 */
public class class_436 {

    public static int ID = 29343;

    public class_436() {
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
            param1.writeShort(16549);
        } catch (IOException e) {
        }
    }
}
