package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class class_789
        implements ServerCommand {


    public static int ID = 10436;

    public Vector<class_653> windows;

    public class_789(Vector<class_653> pWindows) {
        this.windows = pWindows;
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

    protected void writeInternal(DataOutputStream out) {
        try {
            out.writeInt(this.windows.size());
            for (class_653 c : this.windows) {
                c.write(out);
            }
            out.writeShort(-16010);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}