package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 24-03-2015.
 */
public class TechActivationCommand
        implements ServerCommand {

    public TechActivationCommand(TechTypeModule param1, int param2) {
        this.var_2827 = param1;
        this.name_90 = param2;
    }

    public static int ID = 20357;

    public int            name_90;
    public TechTypeModule var_2827;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.method_8(param1);
        } catch (IOException e) {
        }
    }

    protected void method_8(DataOutputStream param1) {
        try {
            param1.writeShort(-17838);
            param1.writeShort(-24881);
            param1.writeInt(this.name_90 << 10 | this.name_90 >>> 22);
            this.var_2827.write(param1);
        } catch (IOException e) {
        }
    }
}
