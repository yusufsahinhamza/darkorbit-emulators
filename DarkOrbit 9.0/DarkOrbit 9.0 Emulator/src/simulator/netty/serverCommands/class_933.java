package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;


public class class_933
        implements ServerCommand {

    public static int ID = 22933;

    public boolean var_1637 = false;

    public int var_1577 = 0;

    public class_933(int param1, boolean param2) {
        this.var_1577 = param1;
        this.var_1637 = param2;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 5;
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
            out.writeBoolean(this.var_1637);
            out.writeInt(this.var_1577 << 9 | this.var_1577 >>> 23);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}