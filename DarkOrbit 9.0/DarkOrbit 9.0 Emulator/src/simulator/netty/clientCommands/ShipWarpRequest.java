package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;
import simulator.netty.ClientCommand;

/**
Created by LEJYONER on 07/01/2018.
*/

public class ShipWarpRequest
        extends ClientCommand {

    public static final int ID = 18133;

    public int shipID = 0;

    public ShipWarpRequest(DataInputStream in) {
        super(in, ID);
    }

    public void readInternal() {
        try {
            this.shipID = in.readInt();
            this.shipID = this.shipID << 7 | this.shipID >>> 25;
            in.readShort();
        } catch (IOException e) {
        }
    }
}