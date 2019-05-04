package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class ActivatePortalCommand
        implements ServerCommand {

    public static int ID       = 30733;
    public        int mapId    = 0;
    public        int var_3576 = 0;

    public ActivatePortalCommand(int param1, int param2) {
        this.mapId = param1;
        this.var_3576 = param2;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.var_3576 << 12 | this.var_3576 >>> 20);
            param1.writeInt(this.mapId << 13 | this.mapId >>> 19);
            param1.writeShort(22459);
        } catch (IOException e) {
        }
    }
}