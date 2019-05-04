package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the lab refinement request

 @author Ordepsousa
 @date 22/07/2014
 @file LabRefinementRequest.java
 @package game.objects.netty.commands */
public class LabRefinementRequest
        extends ClientCommand {

    public static int ID = 17750;
    public OreCountModule toProduce;

    /**
     Constructor
     */
    public LabRefinementRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            in.readShort();
            in.readShort();
            toProduce = new OreCountModule(in);
            toProduce.readInternal();
        } catch (IOException e) {
        }
    }
}