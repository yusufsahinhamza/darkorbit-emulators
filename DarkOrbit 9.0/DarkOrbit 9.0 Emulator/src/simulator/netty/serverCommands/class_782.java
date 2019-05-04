package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_782
        implements ServerCommand {

    public static int    ID       = 21623;
    public        double var_1331 = 0;
    public        int    var_2729 = 0;
    public        double var_2566 = 0;
    public        double var_3371 = 0;

    public class_782(double param1, double param2, double param3, int param4) {
        this.var_3371 = param1;
        this.var_2566 = param2;
        this.var_1331 = param3;
        this.var_2729 = param4;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 28;
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
            out.writeDouble(this.var_1331);
            out.writeInt(this.var_2729 << 7 | this.var_2729 >>> 25);
            out.writeDouble(this.var_2566);
            out.writeDouble(this.var_3371);
            out.writeShort(29519);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}