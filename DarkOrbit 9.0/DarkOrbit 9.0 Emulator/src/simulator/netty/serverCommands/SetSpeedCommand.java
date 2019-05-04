package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class SetSpeedCommand
        implements ServerCommand {

    public SetSpeedCommand(int param1, int param2) {
        this.var_2028 = param1;
        this.var_2042 = param2;
    }

    public static int ID       = 21898;
    public        int var_2042 = 0;
    public        int var_2028 = 0;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.var_2042 << 13 | this.var_2042 >>> 19);
            param1.writeInt(this.var_2028 >>> 12 | this.var_2028 << 20);
        } catch (IOException e) {
        }
    }
}