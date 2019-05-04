package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class class_924
        implements ServerCommand {

    public static int ID = 2802;

    public class_411 var_297;

    public Vector<Integer> var_1785;

    public class_924(class_411 param1, Vector<Integer> param2) {
        this.var_297 = param1;
        this.var_1785 = param2;
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
            this.var_297.write(out);
            out.writeShort(6786);
            out.writeShort(-4600);
            out.writeInt(this.var_1785.size());
            for (Integer i : this.var_1785) {
                out.writeInt(i >>> 15 | i << 17);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}