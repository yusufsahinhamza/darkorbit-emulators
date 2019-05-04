package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;


public class ClientUISlotBarItemModule
        implements ServerCommand {

    public static int    ID       = 15331;
    public        String var_1474 = "";
    public        int    slotId   = 0;

    public ClientUISlotBarItemModule(String param2, int pSlotId) {
        this.slotId = pSlotId;
        this.var_1474 = param2;
    }


    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 6;
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
            out.writeUTF(this.var_1474);
            out.writeInt(this.slotId << 16 | this.slotId >>> 16);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}