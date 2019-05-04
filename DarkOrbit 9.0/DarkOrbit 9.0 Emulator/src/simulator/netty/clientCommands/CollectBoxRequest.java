package simulator.netty.clientCommands;

import java.io.DataInputStream;

import simulator.netty.ClientCommand;

/**
Created by LEJYONER on 15/09/2017.
*/

public class CollectBoxRequest
        extends ClientCommand {

    public static final int ID = 17733;

    /**
     Constructor
     */
    public CollectBoxRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
    }
}

/**
 package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

public class CollectBoxRequest
        extends ClientCommand {

    public static final int id = 29347;
    public String itemHash;

    public CollectBoxRequest(DataInputStream in) {
        super(in, id);
    }

    public void readInternal() {
        try {
            itemHash = in.readUTF();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
}
 */
