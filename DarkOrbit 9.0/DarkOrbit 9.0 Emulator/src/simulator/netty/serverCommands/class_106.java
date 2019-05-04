package simulator.netty.serverCommands;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_106
        implements ServerCommand {

    public class_106() {
    }

    public static int ID = 14422;

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 0;
    }

    public void read(DataInputStream out) {
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

    public void writeInternal(DataOutputStream out) {
    }
}
