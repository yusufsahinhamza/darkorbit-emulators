package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class ShipDeselectionCommand
        implements ServerCommand {

    public static int ID = 93;

    public ShipDeselectionCommand() {
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
            param1.writeShort(-3649);
        } catch (IOException e) {
        }
    }
}
