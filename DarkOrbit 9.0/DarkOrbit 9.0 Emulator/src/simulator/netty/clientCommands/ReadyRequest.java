package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the ready request

 @author Ordepsousa
 @date 22/07/2014
 @file ReadyRequest.java
 @package game.objects.netty.commands */
public class ReadyRequest
        extends ClientCommand {

    public static final int ID = 18168;
    public short readyType;
    public static final short MAP_LOADED  = 0;
    public static final short HERO_LOADED = 1;

    /**
     Constructor
     */
    public ReadyRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            in.readShort();
            this.readyType = in.readShort();
            in.readShort();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
}