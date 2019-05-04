package simulator.netty;

import java.io.DataInputStream;

/**
 Description: Global class for commands

 @author Manulaiko
 @date 27/06/2014
 @file Command.java
 @package game.objects.netty; */
public class ClientCommand {

    public DataInputStream in;
    public short           id, length;

    /**
     Constructor

     @param socket:
     user's socket
     @param id:
     command ID
     */
    public ClientCommand(DataInputStream in, int id) {
        this.in = in;
        this.id = (short) id;
    }

    public int getID() {
        return this.id;
    }

    /**
     Description: Reads command
     */
    public void read() {
        readInternal();
    }

    /**
     Description: Reads specific part of the command(will be overwrited)
     */
    public void readInternal() {
    }
}