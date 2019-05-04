package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class class_495
        implements ServerCommand {

    public static int     ID     = 3715;
    public        boolean remove = false;
    public Vector<class_600> var_2472;

    public class_495(Vector<class_600> param1, boolean pRemove) {
        this.var_2472 = param1;
        this.remove = pRemove;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 5;
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
            out.writeBoolean(this.remove);
            out.writeInt(this.var_2472.size());
            for (class_600 c : this.var_2472) {
                c.write(out);
            }
            out.writeShort(11424);
            out.writeShort(23308);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}