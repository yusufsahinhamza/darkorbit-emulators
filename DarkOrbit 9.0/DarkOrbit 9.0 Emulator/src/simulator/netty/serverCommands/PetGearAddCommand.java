package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 21/09/2017.
*/

public class PetGearAddCommand
        implements ServerCommand {

    public static int ID       = 16710;
    public        PetGearTypeModule gearType;
    public        int level  = 0;
    public 		  int amount = 0;
    public 	boolean enabled = false;

    public PetGearAddCommand(PetGearTypeModule param1, int param2, int param3, boolean param4) {
        this.gearType = param1;
        this.level = param2;
        this.amount = param3;
        this.enabled = param4;
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
            param1.writeShort(-2528);
            param1.writeInt(this.amount >>> 5 | this.amount << 27);
            param1.writeBoolean(this.enabled);
            param1.writeInt(this.level << 4 | this.level >>> 28);
            } catch (IOException e) {
        }
    }
}