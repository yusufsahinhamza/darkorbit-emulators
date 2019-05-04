package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;


public class class_819
        implements ServerCommand {

    public static int ID = 30853;

    public double var_277 = 0;

    public int var_2740 = 0;

    public double duration = 0;

    public class_819(int param1, double param2, double pDuration) {
        this.var_2740 = param1;
        this.var_277 = param2;
        this.duration = pDuration;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 12;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void writeInternal(DataOutputStream out) {
        try {
            out.writeDouble(this.var_277);
            out.writeInt(this.var_2740 >>> 14 | this.var_2740 << 18);
            out.writeShort(30322);
            out.writeDouble(this.duration);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}