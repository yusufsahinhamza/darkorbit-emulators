package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_706
        implements ServerCommand {

    public static int ID = 12520;

    public double var_3553 = 0;

    public double var_3472 = 0;

    public class_706(double param1, double param2) {
        this.var_3553 = param1;
        this.var_3472 = param2;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 16;
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
            out.writeDouble(this.var_3553);
            out.writeDouble(this.var_3472);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}