package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;


public class ClientUISlotBarModule
        implements ServerCommand {

    public static int    ID        = 14403;
    public        String var_536   = "";
    public        String slotBarId = "";
    public        String var_2186  = "";
    public Vector<ClientUISlotBarItemModule> mClientUISlotBarItemModule;


    public ClientUISlotBarModule(String param1, String pSlotBarId, String param3,
                                 Vector<ClientUISlotBarItemModule> pClientUISlotBarItemModule) {
        this.var_2186 = param1;
        this.slotBarId = pSlotBarId;
        this.var_536 = param3;
        this.mClientUISlotBarItemModule = pClientUISlotBarItemModule;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 10;
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
            out.writeShort(31634);
            out.writeUTF(this.var_536);
            out.writeShort(-30847);
            out.writeInt(this.mClientUISlotBarItemModule.size());
            for (ClientUISlotBarItemModule c : this.mClientUISlotBarItemModule) {
                c.write(out);
            }
            out.writeUTF(this.var_2186);
            out.writeUTF(this.slotBarId);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}