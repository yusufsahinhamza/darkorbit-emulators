package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;


public class class_557
        implements ServerCommand {

    public static int ID = 18552;

    public int var_1577 = 0;

    public class_557(int param1) {
        this.var_1577 = param1;
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

    private void writeInternal(DataOutputStream out) {
        try {
            out.writeShort(13690);
            out.writeShort(31530);
            out.writeInt(this.var_1577 << 7 | this.var_1577 >>> 25);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}