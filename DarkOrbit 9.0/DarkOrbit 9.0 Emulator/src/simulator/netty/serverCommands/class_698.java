package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_698
        implements ServerCommand {

    public static int ID = 28603;

    public int level = 0;

    public class_411 var_255;

    public int amount = 0;

    public boolean enabled = false;

    public class_698(class_411 param1, int pLevel, int pAmount, boolean pEnabled) {
        this.var_255 = param1;
        this.level = pLevel;
        this.amount = pAmount;
        this.enabled = pEnabled;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 9;
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
            out.writeInt(this.level << 5 | this.level >>> 27);
            this.var_255.write(out);
            out.writeShort(-27705);
            out.writeShort(20926);
            out.writeInt(this.amount >>> 15 | this.amount << 17);
            out.writeBoolean(this.enabled);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}