package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class class_748
        implements ServerCommand {

    public static int ID = 10674;

    public Vector<class_418> updates;

    public class_748(Vector<class_418> pUpdates) {
        this.updates = pUpdates;
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
            out.writeShort(15550);
            out.writeInt(this.updates.size());
            for (class_418 c : this.updates) {
                c.write(out);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}