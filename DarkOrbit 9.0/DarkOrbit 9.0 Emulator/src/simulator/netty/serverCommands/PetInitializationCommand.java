package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 17-03-2015.
 */
public class PetInitializationCommand
        implements ServerCommand {

    public static int ID = 32119;

    public boolean hasPet;
    public boolean hasFuel;
    public boolean petIsAlive;

    public PetInitializationCommand(boolean param1, boolean param2, boolean param3) {
        this.hasFuel = param1;
        this.petIsAlive = param2;
        this.hasPet = param3;
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
            param1.writeBoolean(this.hasPet);
            param1.writeBoolean(this.hasFuel);
            param1.writeShort(28461);
            param1.writeBoolean(this.petIsAlive);
            param1.writeShort(-21872);
        } catch (IOException e) {
        }
    }

}
