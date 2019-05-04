package simulator.netty.clientCommands;

import java.io.DataInputStream;

import simulator.netty.ClientCommand;

/**
 Created by LEJYONER on 08/10/2017.
*/
public class KillScreenRepairButtonRequest
        extends ClientCommand {

    public static final int ID = 4125;

    /**
     Constructor
     */
    public KillScreenRepairButtonRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
    }
}