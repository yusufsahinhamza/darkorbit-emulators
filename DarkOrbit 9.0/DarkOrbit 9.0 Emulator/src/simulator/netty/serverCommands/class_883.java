package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_883
        implements ServerCommand {

    public static int ID = 26221;

    public int var_1744 = 0;

    public int name_25 = 0;

    public boolean var_1923 = false;

    public class_883(int param1, int param2, boolean param3) {
        this.var_1744 = param1;
        this.name_25 = param2;
        this.var_1923 = param3;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 9;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }

    private void writeInternal(DataOutputStream out) {
        try {
            out.writeShort(32090);
            out.writeShort(14187);
            out.writeInt(this.var_1744 << 4 | this.var_1744 >>> 28);
            out.writeInt(this.name_25 >>> 12 | this.name_25 << 20);
            out.writeBoolean(this.var_1923);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}