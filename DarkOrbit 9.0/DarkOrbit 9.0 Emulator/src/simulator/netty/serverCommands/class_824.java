package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class class_824
        implements ServerCommand {

    public static int ID = 32381;

    public class_411 var_255;

    public Vector<Integer> var_1516;

    public class_824(class_411 param1, Vector<Integer> param2) {
        this.var_255 = param1;
        this.var_1516 = param2;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 4;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream out) {
        try {
            this.var_255.write(out);
            out.writeShort(30272);
            out.writeInt(this.var_1516.size());
            for (Integer i : this.var_1516) {
                out.writeInt(i << 2 | i >>> 30);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}