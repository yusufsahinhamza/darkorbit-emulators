package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;


public class CPUInitializationCommand
        implements ServerCommand {

    public CPUInitializationCommand(boolean param1, boolean param2) {
        this.var_3333 = param1;
        this.var_18 = param2;
    }

    public static int ID = 5533;

    public boolean var_18 = false;

    public boolean var_3333 = false;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeBoolean(this.var_18);
            param1.writeBoolean(this.var_3333);
        } catch (IOException e) {
        }
    }
}