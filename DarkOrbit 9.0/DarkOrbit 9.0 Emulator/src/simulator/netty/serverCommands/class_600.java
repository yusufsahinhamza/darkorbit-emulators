package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;
import java.util.Vector;

import simulator.netty.ServerCommand;

public class class_600
        implements ServerCommand {

    public static int ID = 23100;
    public Vector<Integer> var_2616;
    public int   charCode = 0;
    public short var_1545 = 0;
    public int   var_2477 = 0;

    public static short const_1007 = 7;
    public static short const_592  = 12;
    public static short const_838  = 9;
    public static short LOGOUT     = 6;
    public static short const_1025 = 1;
    public static short const_1712 = 5;
    public static short JUMP       = 0;
    public static short const_1529 = 15;
    public static short const_930  = 4;
    public static short const_1034 = 10;
    public static short const_1338 = 3;
    public static short const_866  = 14;
    public static short const_668  = 8;
    public static short const_1607 = 2;
    public static short const_340  = 11;
    public static short const_1847 = 13;

    public class_600(short param1, Vector<Integer> param2, int param3, int pCharCode) {
        this.var_1545 = param1;
        this.var_2616 = param2;
        this.var_2477 = param3;
        this.charCode = pCharCode;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 10;
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
            out.writeInt(this.var_2616.size());
            for (Integer i : this.var_2616) {
                out.writeInt(i << 15 | i >>> 17);
            }
            out.writeShort(this.charCode);
            out.writeShort(this.var_1545);
            out.writeInt(this.var_2477 << 8 | this.var_2477 >>> 24);
            out.writeShort(-22339);
            out.writeShort(-18238);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}