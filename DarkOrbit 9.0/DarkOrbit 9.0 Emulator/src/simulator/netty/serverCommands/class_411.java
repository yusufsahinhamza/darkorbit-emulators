package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_411
        implements ServerCommand {

    public static short const_1010 = 8;

    public static short const_1487 = 6;

    public static short const_1149 = 1;

    public static short KAMIKAZE = 10;

    public static short const_1361 = 5;

    public static short const_2809 = 12;

    public static short const_240 = 2;

    public static short const_1486 = 0;

    public static short const_1311 = 13;

    public static short const_639 = 3;

    public static short const_990 = 4;

    public static short const_1401 = 7;

    public static short const_228 = 9;

    public static short const_858 = 11;

    public static int ID = 30325;

    public short var_1413 = 0;

    public class_411(short param1) {
        this.var_1413 = param1;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 0;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void writeInternal(DataOutputStream out) {
        try {
            out.writeShort(-17242);
            out.writeShort(this.var_1413);
            out.writeShort(-23550);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}