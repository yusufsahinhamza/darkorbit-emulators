package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class class_406
        implements ServerCommand {

    public static short REMOVE = 1;
    public static short ADD    = 0;
    public static int   ID     = 31172;

    public Vector<Integer> var_2400;

    public int var_1262 = 0;

    public short var_1414 = 0;

    public class_406(short param1, int param2, Vector<Integer> param3) {
        this.var_1414 = param1;
        this.var_1262 = param2;
        this.var_2400 = param3;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 6;
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
            out.writeInt(this.var_2400.size());
            for (Integer i : this.var_2400) {
                out.writeInt(i << 7 | i >>> 25);
            }
            out.writeShort(this.var_1262);
            out.writeShort(this.var_1414);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}