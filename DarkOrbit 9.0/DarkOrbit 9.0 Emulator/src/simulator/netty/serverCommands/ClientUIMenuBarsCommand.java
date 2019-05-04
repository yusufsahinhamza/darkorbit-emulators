package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class ClientUIMenuBarsCommand
        implements ServerCommand {

    public static int ID = 5366;
    public Vector<ClientUIMenuBarModule> mClientUIMenuBarModuleVector;

    public ClientUIMenuBarsCommand(Vector<ClientUIMenuBarModule> pClientUIMenuBarModuleVector) {
        this.mClientUIMenuBarModuleVector = pClientUIMenuBarModuleVector;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 4;
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
            out.writeInt(this.mClientUIMenuBarModuleVector.size());
            for (ClientUIMenuBarModule c : this.mClientUIMenuBarModuleVector) {
                c.write(out);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}