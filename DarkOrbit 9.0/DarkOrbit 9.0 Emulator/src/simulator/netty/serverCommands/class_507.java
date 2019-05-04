package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_507
        implements ServerCommand {

    public static int ID = 5322;

    public int uid = 0;

    public int var_30 = 0;

    public class_507(int pUID, int param2) {
        this.uid = pUID;
        this.var_30 = param2;
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

    public void writeInternal(DataOutputStream out) {
        try {
            out.writeInt(this.uid << 4 | this.uid >>> 28);
            out.writeInt(this.var_30 << 14 | this.var_30 >>> 18);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}