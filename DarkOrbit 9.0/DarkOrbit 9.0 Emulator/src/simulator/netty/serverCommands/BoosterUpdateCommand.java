package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;
import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 11/12/2017.
*/

public class BoosterUpdateCommand
        implements ServerCommand {

    public static int ID = 1164;
    public Vector<BoosterTypesModule> boosterTypes;
    public int value;
    public BoosterAttributeType attributeType;
    
    public BoosterUpdateCommand(BoosterAttributeType param1, short param2, Vector<BoosterTypesModule> param3) {
    	this.attributeType = param1;
        this.value = param2;
        this.boosterTypes = param3;
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
            param1.writeInt(this.boosterTypes.size());
            for (BoosterTypesModule c : this.boosterTypes) {
                c.write(param1);
            }                        
            param1.writeFloat(this.value);
            param1.writeShort(-12413);
            this.attributeType.write(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}