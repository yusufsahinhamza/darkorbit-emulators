package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

/**
 Description: This is the kill screen option type module

 @author Ordepsousa
 @date 22/07/2014
 @file KillScreenOptionTypeModule.java
 @package game.objects.netty.commands */
public class KillScreenOptionTypeModule {

    public DataInputStream in;
    public static int id = 25757;
    public short repairTypeValue;
    public static short FREE_PHOENIX                 = 0;
    public static short BASIC_REPAIR                 = 1;
    public static short AT_JUMPGATE_REPAIR           = 2;
    public static short AT_DEATHLOCATION_REPAIR      = 3;
    public static short AT_SECTOR_CONTROL_SPAWNPOINT = 4;
    public static short EXIT_SECTOR_CONTROL          = 5;
    public static short BASIC_FULL_REPAIR            = 6;

    /**
     Constructor
     */
    public KillScreenOptionTypeModule(DataInputStream in) {
        this.in = in;
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            repairTypeValue = in.readShort();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
}