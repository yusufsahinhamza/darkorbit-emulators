package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_907
        implements ServerCommand {

    public static short NONE = 0;

    public static short const_1645 = 1;

    public static short const_633 = 2;

    public static short const_361 = 3;

    public static int ID = 2741;

    public short type = 0;

    public class_907(short pType) {
        this.type = pType;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 0;
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
            out.writeShort(this.type);
            out.writeShort(22311);
            out.writeShort(-2661);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}