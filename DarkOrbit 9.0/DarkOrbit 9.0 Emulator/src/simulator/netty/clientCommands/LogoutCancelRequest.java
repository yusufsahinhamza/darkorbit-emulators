package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

public class LogoutCancelRequest
        extends ClientCommand {

    public static final int ID = 12738;

    /**
     Constructor
     */
    public LogoutCancelRequest(DataInputStream in) {
        super(in, ID);
    }

    public void readInternal(DataInputStream in) {
        try {
            in.readShort();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}