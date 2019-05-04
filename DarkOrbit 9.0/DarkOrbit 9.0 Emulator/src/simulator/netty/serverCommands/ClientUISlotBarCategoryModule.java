package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class ClientUISlotBarCategoryModule
        implements ServerCommand {

    public static int ID = 16643;
    public Vector<ClientUISlotBarCategoryItemModule> mClientUISlotBarCategoryItemModuleVector;
    public String var_3036 = "";

    public ClientUISlotBarCategoryModule(String out,
                                         Vector<ClientUISlotBarCategoryItemModule> pClientUISlotBarCategoryItemModuleVector) {
        this.var_3036 = out;
        this.mClientUISlotBarCategoryItemModuleVector = pClientUISlotBarCategoryItemModuleVector;
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
            out.writeUTF(this.var_3036);
            out.writeInt(this.mClientUISlotBarCategoryItemModuleVector.size());
            for (ClientUISlotBarCategoryItemModule c : this.mClientUISlotBarCategoryItemModuleVector) {
                c.write(out);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}