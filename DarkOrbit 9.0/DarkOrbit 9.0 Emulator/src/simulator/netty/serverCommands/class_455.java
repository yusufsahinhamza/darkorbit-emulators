package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_455
        implements ServerCommand {

    public class_455(boolean param1, boolean param2, boolean param3) {

        this.var_3246 = param1;
        this.var_2204 = param2;
        this.var_1892 = param3;
    }

    public static int ID = 17580;

    public boolean var_2204 = false;

    public boolean var_3246 = false;

    public boolean var_1892 = false;

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 3;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
        }
    }

    private void writeInternal(DataOutputStream out) {
        try {
            out.writeShort(-1466);
            out.writeBoolean(this.var_2204);
            out.writeBoolean(this.var_3246);
            out.writeBoolean(this.var_1892);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}