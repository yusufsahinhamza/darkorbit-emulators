package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

/**
 Description: This is the lab item module

 @author Ordepsousa
 @date 22/07/2014
 @file LabItemModule.java
 @package game.objects.netty.commands */
public class LabItemModule {

    public DataInputStream in;
    public static int id = 29012;
    public short itemValue;
    public static final short LASER   = 0;
    public static final short ROCKETS = 1;
    public static final short DRIVING = 2;
    public static final short SHIELD  = 3;

    /**
     Constructor
     */
    public LabItemModule(DataInputStream in) {
        this.in = in;
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            itemValue = in.readShort();
            in.readShort();
            in.readShort();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
}