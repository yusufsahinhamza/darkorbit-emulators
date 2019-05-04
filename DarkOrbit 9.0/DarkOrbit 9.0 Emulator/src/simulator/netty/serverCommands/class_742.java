package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_742
        implements ServerCommand {

    public static int ID = 14121;

    public boolean blocked = false;

    public class_742(boolean pBlocked) {

        this.blocked = pBlocked;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 1;
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
            out.writeBoolean(this.blocked);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}