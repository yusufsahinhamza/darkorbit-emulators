package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the lab update request

 @author Ordepsousa
 @date 22/07/2014
 @file LabUpdateRequest.java
 @package game.objects.netty.commands */
public class LabUpdateRequest
        extends ClientCommand {

    public static final int ID = 877;

    /**
     Constructor
     */
    public LabUpdateRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            in.readShort();
            in.readShort();
        } catch (IOException e) {
        }
    }
}