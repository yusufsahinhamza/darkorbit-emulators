package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

import simulator.netty.ClientCommand;

/**
 Description: This is the ammunition type module

 @author Ordepsousa
 @date 21/07/2014
 @file AmmunitionTypeModule.java
 @package game.objects.netty.ServerCommands */
public class AmmunitionTypeModule
        extends ClientCommand {

    public static int id = 15629;
    public short typeValue;
    public static final short ROCKET         = 0;
    public static final short TORPEDO        = 1;
    public static final short WIZARD         = 2;
    public static final short PRANK          = 3;
    public static final short BATTERY        = 4;
    public static final short FIREWORK       = 5;
    public static final short FIREWORK_1     = 6;
    public static final short FIREWORK_2     = 7;
    public static final short FIREWORK_3     = 8;
    public static final short MINE           = 9;
    public static final short MINE_EMP       = 10;
    public static final short MINE_SAB       = 11;
    public static final short MINE_DD        = 12;
    public static final short MINE_SL        = 13;
    public static final short SMARTBOMB      = 14;
    public static final short INSTANT_SHIELD = 15;
    public static final short PLASMA         = 16;
    public static final short EMP            = 17;
    public static final short LASER_AMMO     = 18;
    public static final short ROCKET_AMMO    = 19;
    public static final short RSB            = 20;
    public static final short HELLSTORM      = 21;
    public static final short UBER_ROCKET    = 22;
    public static final short ECO_ROCKET     = 23;
    public static final short SAR01          = 24;
    public static final short SAR02          = 25;
    public static final short X1             = 26;
    public static final short X2             = 27;
    public static final short X3             = 28;
    public static final short X4             = 29;
    public static final short SAB            = 30;
    public static final short CBO            = 31;
    public static final short R310           = 32;
    public static final short PLT2026        = 33;
    public static final short PLT2021        = 34;
    public static final short PLT3030        = 35;
    public static final short BDR1211        = 36;
    public static final short DECELERATION   = 37;
    public static final short CBR            = 38;
    public static final short HITAC_LASER    = 39;
    public static final short JOB100         = 40;
    public static final short BDR1212        = 41;

    /**
     Constructor
     */
    public AmmunitionTypeModule(DataInputStream in) {
        super(in, id);
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