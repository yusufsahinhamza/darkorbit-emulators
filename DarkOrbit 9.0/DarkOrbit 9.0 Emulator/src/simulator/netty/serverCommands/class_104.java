package simulator.netty.serverCommands;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_104
        implements ServerCommand {

    public static int     ID    = 2815;
    public        boolean close = false;

    /**
     Constructor

     @param out
     */
    public class_104(boolean pClose) {
        this.close = pClose;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 1;
    }

    public void read(DataInputStream out) {
        try {
            this.close = out.readBoolean();
        } catch (IOException e) {
            e.printStackTrace();
        }
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
            out.writeBoolean(this.close);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
