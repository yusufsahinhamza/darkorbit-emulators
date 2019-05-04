package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the version request

 @author Ordepsousa
 @date 22/07/2014
 @file VersionRequest.java
 @package game.objects.netty.commands */
public class VersionRequest
        extends ClientCommand {

    public static final int ID = 666;

    public int major, build;
    public String minor;

    /**
     Constructor
     */
    public VersionRequest(DataInputStream in) {
        super(in, ID);
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            major = in.readInt();
            minor = in.readUTF();
            build = in.readInt();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}