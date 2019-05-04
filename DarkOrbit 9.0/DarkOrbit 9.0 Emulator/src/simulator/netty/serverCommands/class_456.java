package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 16-03-2015.
 */
public class class_456
        implements ServerCommand {

    public static int ID = 8830;

    public Vector<class_561> var_3235;

    public class_456(Vector<class_561> param1) {
        this.var_3235 = param1;
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
            param1.writeInt(this.var_3235.size());
            for (class_561 c : this.var_3235) {
                c.write(param1);
            }
        } catch (IOException e) {
        }
    }
}
