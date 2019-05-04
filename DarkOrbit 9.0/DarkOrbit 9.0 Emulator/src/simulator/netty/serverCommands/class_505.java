package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;


public class class_505
        implements ServerCommand {

    public static int ID = 24569;

    public int var_485  = 0;
    public int var_2046 = 0;
    public int var_1518 = 0;

    public class_505(int param1, int param2, int param3) {
        this.var_1518 = param1;
        this.var_2046 = param2;
        this.var_485 = param3;
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
            param1.writeInt(this.var_485 << 1 | this.var_485 >>> 31);
            param1.writeInt(this.var_2046 >>> 7 | this.var_2046 << 25);
            param1.writeInt(this.var_1518 >>> 1 | this.var_1518 << 31);
        } catch (IOException e) {
        }
    }
}