package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 09/01/2018.
*/

public class PetGearSelectCommand
        implements ServerCommand {

    public static int ID = 31289;

    public PetGearTypeModule gearType;   
    public Vector<Integer> optionalParams;
   
    public PetGearSelectCommand(PetGearTypeModule param1, Vector<Integer> param2) {
    	this.gearType = param1;
    	this.optionalParams = param2;
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
            param1.writeInt(this.optionalParams.size());
            for (Integer i : this.optionalParams) {
                param1.writeInt(i >>> 16 | i << 16);
            }
            this.gearType.write(param1);
            param1.writeShort(6186);
            param1.writeShort(710);
        } catch (IOException e) {
        }
    }

}
