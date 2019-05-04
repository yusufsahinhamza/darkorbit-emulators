package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_641
        implements ServerCommand {

    public static int ID = 11673;

    public int var_3395 = 0;

    public int var_204 = 0;

    public class_641(int param1, int param2) {
        this.var_204 = param1;
        this.var_3395 = param2;
    }


    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 8;
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
            out.writeInt(this.var_3395 << 10 | this.var_3395 >>> 22);
            out.writeInt(this.var_204 >>> 14 | this.var_204 << 18);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}