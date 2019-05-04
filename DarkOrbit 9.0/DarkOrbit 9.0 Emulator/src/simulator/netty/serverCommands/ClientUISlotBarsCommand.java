package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class ClientUISlotBarsCommand
        implements ServerCommand {

    public static int    ID       = 8220;
    public        String var_2964 = "";
    public Vector<ClientUISlotBarModule>         mClientUISlotBarVector;
    public Vector<ClientUISlotBarCategoryModule> mClientUISlotBarCategoryVector;

    public ClientUISlotBarsCommand(String param2, Vector<ClientUISlotBarModule> pClientUISlotBarVector,
                                   Vector<ClientUISlotBarCategoryModule> pClientUISlotBarCategoryVector) {
        this.mClientUISlotBarCategoryVector = pClientUISlotBarCategoryVector;
        this.var_2964 = param2;
        this.mClientUISlotBarVector = pClientUISlotBarVector;
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
            out.writeInt(this.mClientUISlotBarCategoryVector.size());
            for (ClientUISlotBarCategoryModule c : this.mClientUISlotBarCategoryVector) {
                c.write(out);
            }
            out.writeInt(this.mClientUISlotBarVector.size());
            for (ClientUISlotBarModule c : this.mClientUISlotBarVector) {
                c.write(out);
            }
            out.writeUTF(this.var_2964);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}