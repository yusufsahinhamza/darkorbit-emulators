package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 11/12/2017.
*/

public class BoosterAttributeType
        implements ServerCommand {

    public static int ID = 20371;

    public static short EP               = 0;
    public static short HONOUR           = 1;
    public static short DAMAGE           = 2;
    public static short SHIELD           = 3;
    public static short REPAIR           = 4;
    public static short SHIELDRECHARGE   = 5;
    public static short RESOURCE         = 6;
    public static short MAXHP            = 7;
    public static short ABILITY_COOLDOWN = 8;
    public static short BONUSBOXES       = 9;
    public static short QUESTREWARD      = 10;
    
    public short attributeType;
    
    public BoosterAttributeType(short param1) {
    	this.attributeType = param1;
    }
    
    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(this.attributeType);
            param1.writeShort(7272);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}