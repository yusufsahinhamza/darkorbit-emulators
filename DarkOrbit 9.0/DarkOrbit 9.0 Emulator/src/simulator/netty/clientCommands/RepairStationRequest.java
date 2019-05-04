package simulator.netty.clientCommands;

import java.io.DataInputStream;

import simulator.netty.ClientCommand;

/**
 Created by LEJYONER on 16/09/2017.
*/
public class RepairStationRequest
        extends ClientCommand {

    public static final int ID = 22044;

    /**
     Constructor
     */
    public RepairStationRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
    }
}