package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_625
        implements ServerCommand {

    public static int ID = 17841;

    public int name_29 = 0;

    public class_625(int param1) {
        this.name_29 = param1;
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
            out.writeShort(-27284);
            out.writeInt(this.name_29 >>> 10 | this.name_29 << 22);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}