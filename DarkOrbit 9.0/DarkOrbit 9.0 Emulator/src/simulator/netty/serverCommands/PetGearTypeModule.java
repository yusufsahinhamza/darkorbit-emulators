package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 21/09/2017.
*/

public class PetGearTypeModule
        implements ServerCommand {

    public static final short BEHAVIOR = 0;    
    public static final short PASSIVE = 1;   
    public static final short GUARD = 2;   
    public static final short GEAR = 3;   
    public static final short AUTO_LOOT = 4;   
    public static final short AUTO_RESOURCE_COLLECTION = 5;  
    public static final short ENEMY_LOCATOR = 6;   
    public static final short RESOURCE_LOCATOR = 7;   
    public static final short TRADE_POD = 8;   
    public static final short REPAIR_PET = 9;   
    public static final short KAMIKAZE = 10;  
    public static final short COMBO_SHIP_REPAIR = 11;   
    public static final short COMBO_GUARD = 12;   
    public static final short ADMIN = 13;
   
    public static int ID = 25441;

    public short typeValue = 0;
    
    public PetGearTypeModule(short param1) {
    	this.typeValue = param1;
    }

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(typeValue);
        } catch (IOException e) {
        }
    }
}