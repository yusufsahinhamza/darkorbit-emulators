package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import simulator.netty.ServerCommand;

/**
Created by LEJYONER on 09/01/2018.
*/

public class PetBlockUICommand
        implements ServerCommand {

    public static int ID = 7613;

    public boolean blocked = false;
   
    public PetBlockUICommand(boolean param1) {
        this.blocked = param1;
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
            param1.writeBoolean(this.blocked);
            param1.writeShort(15390);
        } catch (IOException e) {
        }
    }

}
