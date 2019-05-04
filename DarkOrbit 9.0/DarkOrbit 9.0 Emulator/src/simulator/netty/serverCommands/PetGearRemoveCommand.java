package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 21/09/2017.
*/

public class PetGearRemoveCommand
        implements ServerCommand {

    public static int ID       = 30680;
    public        PetGearTypeModule gearType;
    public        int level  = 0;
    public 		  int amount = 0;

    public PetGearRemoveCommand(PetGearTypeModule param1, int param2, int param3) {
        this.gearType = param1;
        this.level = param2;
        this.amount = param3;
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
            this.gearType.write(param1);
            param1.writeInt(this.level << 3 | this.level >>> 29);
            param1.writeShort(9304);
            param1.writeInt(this.amount >>> 15 | this.amount << 17);
            } catch (IOException e) {
        }
    }
}