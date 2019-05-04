package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

public class PortalJumpRequest
        extends ClientCommand {

    public PortalJumpRequest(DataInputStream in) {
        super(in, ID);
    }

    public static final int ID = 7018;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
    }
}