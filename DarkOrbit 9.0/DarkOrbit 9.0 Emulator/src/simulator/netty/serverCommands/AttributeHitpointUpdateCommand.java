package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

/**
 Created by Pedro on 23-03-2015.
 */
public class AttributeHitpointUpdateCommand
        implements ServerCommand {

    public AttributeHitpointUpdateCommand(int param1, int param2, int param3, int param4) {
        this.name_60 = param1;
        this.name_25 = param2;
        this.var_1454 = param3;
        this.var_2536 = param4;
    }

    public static int ID = 28524;

    public int name_60  = 0;
    public int name_25  = 0;
    public int var_2536 = 0;
    public int var_1454 = 0;

    public void write(DataOutputStream param1) {
        try {
            param1.writeShort(ID);
            this.writeInternal(param1);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream param1) {
        try {
            param1.writeInt(this.name_60 << 6 | this.name_60 >>> 26);
            param1.writeInt(this.name_25 >>> 2 | this.name_25 << 30);
            param1.writeInt(this.var_2536 >>> 14 | this.var_2536 << 18);
            param1.writeInt(this.var_1454 >>> 8 | this.var_1454 << 24);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
