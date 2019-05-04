package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

public class FactionModule
        extends class_418 {

    public static int ID = 2430;

    public short faction = NONE;

    public static short NONE = 0;
    public static short MMO  = 1;
    public static short VRU  = 3;
    public static short EIC  = 2;

    public FactionModule(short pFaction) {
        this.faction = pFaction;
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
            super.writeInternal(out);
            out.writeShort(2035);
            out.writeShort(this.faction);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}