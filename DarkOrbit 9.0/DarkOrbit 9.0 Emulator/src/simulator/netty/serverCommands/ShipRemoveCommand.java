package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 03-04-2015.
 */
public class ShipRemoveCommand
        implements ServerCommand {

    public static int ID = 20034;

    public int name_90 = 0;

    public ShipRemoveCommand(int param1) {
        this.name_90 = param1;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.name_90 << 13 | this.name_90 >>> 19);
            param1.writeShort(4726);
        } catch (IOException e) {
        }
    }
}
