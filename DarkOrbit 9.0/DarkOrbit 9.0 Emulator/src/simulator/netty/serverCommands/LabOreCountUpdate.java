package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 14-03-2015.
 */
public class LabOreCountUpdate
        implements ServerCommand {

    public LabOreCountUpdate(Vector<OreCountModuleCommand> param1) {
        this.var_2166 = param1;
    }

    public static int ID = 10721;

    public Vector<OreCountModuleCommand> var_2166;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(11312);
            param1.writeInt(this.var_2166.size());
            for (OreCountModuleCommand c : this.var_2166) {
                c.write(param1);
            }
            param1.writeShort(-16827);
        } catch (IOException e) {
        }
    }
}
