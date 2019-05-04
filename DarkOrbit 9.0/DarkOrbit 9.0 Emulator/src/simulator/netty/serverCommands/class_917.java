package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_917
        implements ServerCommand {

    public class_917(class_502 param1, class_488 param2) {
        this.var_819 = param1;
        this.var_3334 = param2;
    }

    public static int ID = 31035;
    public class_488 var_3334;
    public class_502 var_819;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeShort(-1905);
            this.var_3334.write(param1);
            this.var_819.write(param1);
        } catch (IOException e) {
        }
    }
}