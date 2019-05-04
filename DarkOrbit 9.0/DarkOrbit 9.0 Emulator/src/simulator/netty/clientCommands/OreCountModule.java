package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

/**
 Description: This is the ore count module

 @author Ordepsousa
 @date 22/07/2014
 @file OreCountModule.java
 @package game.objects.netty.commands */
public class OreCountModule {

    public DataInputStream in;
    public static int ID = 8800;
    public OreTypeModule oreType;
    public Double        count;

    /**
     Constructor
     */
    public OreCountModule(DataInputStream in) {
        this.in = in;
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            count = in.readDouble();
            in.readShort();
            oreType = new OreTypeModule(in);
            oreType.readInternal();
            in.readShort();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
}