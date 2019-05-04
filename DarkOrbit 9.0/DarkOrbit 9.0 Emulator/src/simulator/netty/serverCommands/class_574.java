package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_574
        implements ServerCommand {

    public static int ID = 29823;

    public int level = 0;

    public class_411 var_255;

    public int amount = 0;

    public class_574(class_411 param1, int pLevel, int pAmount) {
        this.var_255 = param1;
        this.level = pLevel;
        this.amount = pAmount;
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

    private void writeInternal(DataOutputStream out) {
        try {
            out.writeInt(this.level >>> 8 | this.level << 24);
            out.writeShort(-30238);
            this.var_255.write(out);
            out.writeInt(this.amount << 2 | this.amount >>> 30);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}