package simulator.netty.serverCommands;


import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

/**
 Created by Pedro on 22-03-2015.
 */
public class AvailableModulesCommand {

    public AvailableModulesCommand(Vector<StationModuleModule> param1) {
        this.modules = param1;
    }

    public static int ID = 4301;

    public Vector<StationModuleModule> modules;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.modules.size());
            for (StationModuleModule c : this.modules) {
                c.write(param1);
            }
        } catch (IOException e) {
        }
    }
}
