package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the lab update item request

 @author Ordepsousa
 @date 22/07/2014
 @file LabUpdateItemRequest.java
 @package game.objects.netty.commands */
public class LabUpdateItemRequest
        extends ClientCommand {

    public static int ID = 8327;
    public LabItemModule  itemToUpdate;
    public OreCountModule updateWith;

    /**
     Constructor
     */
    public LabUpdateItemRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            in.readShort();
            itemToUpdate = new LabItemModule(in);
            itemToUpdate.readInternal();
            in.readShort();
            updateWith = new OreCountModule(in);
            updateWith.readInternal();
        } catch (IOException e) {
        }
    }
}