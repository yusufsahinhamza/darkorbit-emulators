package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class EquipReadyCommand
        implements ServerCommand {

    public EquipReadyCommand(boolean param1) {
        this.ready = param1;
    }

    public static int     ID    = 439;
    public        boolean ready = false;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeBoolean(this.ready);
            param1.writeShort(-19324);
        } catch (IOException e) {
        }
    }
}