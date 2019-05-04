 package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
Created by LEJYONER on 11/12/2017.
*/

public class GroupSendRequest
        extends ClientCommand {

    public static final int ID = 30020;
    public String name = "";
    
    public GroupSendRequest(DataInputStream in) {
        super(in, ID);
    }

    public void readInternal() {
        try {
        	name = in.readUTF();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}