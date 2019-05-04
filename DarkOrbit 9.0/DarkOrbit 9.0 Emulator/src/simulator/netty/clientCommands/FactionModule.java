package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

/**
 Description: This is the faction module

 @author Asus
 @date 22/07/2014
 @file FactionModule.java
 @package game.objects.netty.commands */
public class FactionModule {

    public DataInputStream in;
    public static int   id   = 15721;
    public static short NONE = 0;
    public static short MMO  = 1;
    public static short EIC  = 2;
    public static short VRU  = 3;
    public short faction;

    /**
     Constructor
     */
    public FactionModule(DataInputStream in) {
        this.in = in;
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            faction = in.readShort();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
}
