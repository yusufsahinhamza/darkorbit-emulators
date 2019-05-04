package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 07/01/2018.
*/

public class BoosterFoundOnMapCommand
        implements ServerCommand {

    public static int ID = 2373;
    
    public BoosterTypesModule boosterType;   
    public int hours = 0;
    
    public BoosterFoundOnMapCommand(BoosterTypesModule param1, int param2) {
        this.boosterType = param1;
        this.hours = param2;
    }
    
    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void writeInternal(DataOutputStream param1) {
        try {
            this.boosterType.write(param1);
            param1.writeShort(-30707);
            param1.writeShort(25213);
            param1.writeInt(this.hours << 5 | this.hours >>> 27);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}