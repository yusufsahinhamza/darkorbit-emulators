package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

/**
 Description: This is the ore type module

 @author Ordepsousa
 @date 22/07/2014
 @file OreTypeModule.java
 @package game.objects.netty.commands */
public class OreTypeModule {

    public DataInputStream in;
    public static int id = 20950;
    public short typeValue;
    public static final short PROMETIUM = 0;
    public static final short ENDURIUM  = 1;
    public static final short TERBIUM   = 2;
    public static final short XENOMIT   = 3;
    public static final short PROMETID  = 4;
    public static final short DURANIUM  = 5;
    public static final short PROMERIUM = 6;
    public static final short SEPROM    = 7;
    public static final short PALLADIUM = 8;

    /**
     Constructor
     */
    public OreTypeModule(DataInputStream in) {
        this.in = in;
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            typeValue = in.readShort();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
}