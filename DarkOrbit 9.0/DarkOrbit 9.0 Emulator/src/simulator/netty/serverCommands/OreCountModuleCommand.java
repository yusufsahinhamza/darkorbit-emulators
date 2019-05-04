package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

/**
 Created by Pedro on 14-03-2015.
 */
public class OreCountModuleCommand {

    public OreCountModuleCommand(OreTypeModuleCommand param1, long param2) {
        this.var_3261 = param1;
        this.count = param2;
    }

    public static int ID = 8800;

    public long count = 0;

    public OreTypeModuleCommand var_3261;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeDouble(this.count);
            this.var_3261.write(param1);
            param1.writeShort(-30285);
        } catch (IOException e) {
        }
    }
}
