package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 10/01/2018.
*/

public class PetLocatorGearInitializationCommand
        implements ServerCommand {

    public static int ID = 5319;

    public PetGearTypeModule locatorType;   
    public Vector<Integer> possibleTargetValues;
   
    public PetLocatorGearInitializationCommand(PetGearTypeModule param1, Vector<Integer> param2) {
    	this.locatorType = param1;
    	this.possibleTargetValues = param2;
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
            param1.writeInt(this.possibleTargetValues.size());
            for (Integer i : this.possibleTargetValues) {
               param1.writeInt(i << 1 | i >>> 31);
            }
            param1.writeShort(-4739);
            this.locatorType.write(param1);
        } catch (IOException e) {
        }
    }

}
