package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 14-03-2015.
 */
public class class_802
        implements ServerCommand {

    public static int ID = 15521;

    public Vector<class_667> windows;

    public class_802(Vector<class_667> param1) {
        this.windows = param1;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.method_8(param1);
        } catch (IOException e) {
        }
    }

    protected void method_8(DataOutputStream param1) {
        try {
            param1.writeInt(windows.size());
            for (class_667 c : windows) {
                c.write(param1);
            }
        } catch (IOException e) {
        }
    }
}
