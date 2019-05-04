package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 14-03-2015.
 */
public class SpacemapWindowUpdate
        implements ServerCommand {

    public SpacemapWindowUpdate(boolean param1, boolean param2) {
        this.upperMaps = param1;
        this.jumpCpu = param2;
    }

    public static int ID = 4972;

    public boolean jumpCpu   = false;
    public boolean upperMaps = false;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(-25513);
            param1.writeBoolean(this.jumpCpu);
            param1.writeBoolean(this.upperMaps);
        } catch (IOException e) {
        }
    }
}
