package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the legacy module

 @author Asus
 @date 23/07/2014
 @file LegacyModule.java
 @package game.objects.netty.commands */
public class LegacyModuleRequest
        extends ClientCommand {

    public static final int ID = 15145;

    public String message;

    /**
     Constructor
     */
    public LegacyModuleRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            message = in.readUTF();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
}
