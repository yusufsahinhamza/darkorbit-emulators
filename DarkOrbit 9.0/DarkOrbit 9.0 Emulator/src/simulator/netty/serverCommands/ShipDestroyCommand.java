package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class ShipDestroyCommand
        implements ServerCommand {

    public static final int ID = 23799;

    public int var_731 = 0;

    public int mMapEntityId = 0;

    public ShipDestroyCommand(int pMapEntityId, int param2) {
        this.mMapEntityId = pMapEntityId;
        this.var_731 = param2;
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

    protected void writeInternal(DataOutputStream out) {
        try {
            out.writeInt(this.var_731 << 2 | this.var_731 >>> 30);
            out.writeShort(-26844);
            out.writeInt(this.mMapEntityId << 7 | this.mMapEntityId >>> 25);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}