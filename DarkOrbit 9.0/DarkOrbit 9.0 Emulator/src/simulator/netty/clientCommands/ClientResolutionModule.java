package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

/**
 Description: This is the client resolution module

 @author Ordepsousa
 @date 22/07/2014
 @file ClientResolutionModule.java
 @package game.objects.netty.commands */
public class ClientResolutionModule {

    public DataInputStream in;
    public static int id = 32511;
    public int resolutionId, width, height;

    /**
     Constructor
     */
    public ClientResolutionModule(DataInputStream in) {
        this.in = in;
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            resolutionId = in.readInt();
            width = in.readInt();
            height = in.readInt();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
}