package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class ShipSelectionCommand
        implements ServerCommand {

    public static int     ID       = 23689;
    public        int     name_84  = 0;
    public        int     name_55  = 0;
    public        int     shield   = 0;
    public        int     var_1556 = 0;
    public        int     var_2984 = 0;
    public        int     name_68  = 0;
    public        int     name_25  = 0;
    public        int     var_1081 = 0;
    public        boolean var_2318 = false;

    public ShipSelectionCommand(int param1, int param2, int param3, int param4, int param5, int param6, int param7,
                                int param8, boolean param9) {
        this.name_84 = param1;
        this.var_2984 = param2;
        this.shield = param3;
        this.name_68 = param4;
        this.name_55 = param5;
        this.name_25 = param6;
        this.var_1556 = param7;
        this.var_1081 = param8;
        this.var_2318 = param9;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 33;
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
            out.writeInt(this.name_68 >>> 7 | this.name_68 << 25);
            out.writeInt(this.var_1081 << 5 | this.var_1081 >>> 27);
            out.writeShort(-16109);
            out.writeInt(this.name_55 >>> 13 | this.name_55 << 19);
            out.writeInt(this.name_25 >>> 12 | this.name_25 << 20);
            out.writeInt(this.name_84 >>> 2 | this.name_84 << 30);
            out.writeBoolean(this.var_2318);
            out.writeInt(this.var_2984 << 4 | this.var_2984 >>> 28);
            out.writeInt(this.shield >>> 15 | this.shield << 17);
            out.writeInt(this.var_1556 >>> 4 | this.var_1556 << 28);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}