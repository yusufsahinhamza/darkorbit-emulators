package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;


public class ClientUISlotBarCategoryItemTimerStateModule
        implements ServerCommand {

    public static int   ID         = 29841;
    public static short READY      = 0;
    public static short ACTIVE     = 1;
    public static short short_2168 = 2;

    public short var_1238 = 0;

    public ClientUISlotBarCategoryItemTimerStateModule(short param1) {
        this.var_1238 = param1;
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
            out.writeShort(-10628);
            out.writeShort(this.var_1238);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}