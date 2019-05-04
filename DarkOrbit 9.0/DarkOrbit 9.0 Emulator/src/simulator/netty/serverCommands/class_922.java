package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_922
        implements ServerCommand {

    public class_922(boolean param1, boolean param2, boolean param3, boolean param4) {
        this.var_812 = param1;
        this.var_1070 = param2;
        this.showRequests = param3;
        this.var_913 = param4;
    }

    public static int ID = 22675;

    public boolean showRequests;
    public boolean var_812;
    public boolean var_1070;
    public boolean var_913;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeBoolean(this.showRequests);
            param1.writeBoolean(this.var_812);
            param1.writeShort(17862);
            param1.writeBoolean(this.var_1070);
            param1.writeBoolean(this.var_913);
        } catch (IOException e) {
        }
    }
}