package simulator.netty.clientCommands;

import java.io.DataInputStream;
import java.io.IOException;

/**
 Description: This is the button id module

 @author Asus
 @date 22/07/2014
 @file ButtonIDModule.java
 @package game.objects.netty.commands */
public class ButtonIDModule {

    public DataInputStream in;
    public static int   id                  = 8712;
    public static short LASER_x1            = 0;
    public static short LASER_x2            = 1;
    public static short LASER_x3            = 2;
    public static short LASER_x4            = 3;
    public static short LASER_SAB           = 4;
    public static short LASER_RSB           = 5;
    public static short ROCKET_1            = 6;
    public static short ROCKET_2            = 7;
    public static short ROCKET_3            = 8;
    public static short WIZARD              = 9;
    public static short PLASMA              = 10;
    public static short DECELERATION_ROCKET = 11;
    public static short EMP                 = 12;
    public static short MINE_ACM            = 13;
    public static short MINE_EMP            = 14;
    public static short MINE_SAB            = 15;
    public static short MINE_DD             = 16;
    public static short ROCKET_LAUNCHER     = 17;
    public static short HELLSTORM_01        = 18;
    public static short UBR_100             = 19;
    public static short ECO_10              = 20;
    public static short CPU_DRONE_REPAIR    = 21;
    public static short CPU_AIM             = 22;
    public static short CPU_AROL            = 23;
    public static short CPU_CLOAK           = 24;
    public static short CPU_JUMP            = 25;
    public static short CPU_REPAIR_ROBOT    = 26;
    public static short CPU_HM7             = 27;
    public static short CPU_AMMOBUY         = 28;
    public static short JUMP                = 29;
    public static short FAST_REPAIR         = 30;
    public static short LOGOUT              = 31;
    public static short MENU_LASER          = 32;
    public static short MENU_ROCKET         = 33;
    public static short MENU_EXPLOSIVES     = 34;
    public static short MENU_CPU            = 35;
    public static short MENU_EXTRAS         = 36;
    public static short MENU_TECHS          = 37;
    public static short MENU_SKILLS         = 38;
    public short idValue;

    /**
     Constructor
     */
    public ButtonIDModule(DataInputStream in) {
        this.in = in;
    }

    /**
     Description: Reads command
     */
    public void readInternal() {
        try {
            idValue = in.readShort();
        } catch (IOException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
}
