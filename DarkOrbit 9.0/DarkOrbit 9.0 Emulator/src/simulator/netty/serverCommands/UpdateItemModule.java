package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 03-04-2015.
 */
public class UpdateItemModule
        implements ServerCommand {

    public UpdateItemModule(LabItemModule param1, OreCountModuleCommand param2) {
        this.var_120 = param1;
        this.var_2005 = param2;
    }

    public static int ID = 2575;

    public LabItemModule var_120;

    public OreCountModuleCommand var_2005;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(7342);
            this.var_120.write(param1);
            this.var_2005.write(param1);
            param1.writeShort(-20770);
        } catch (IOException e) {
        }
    }
}
