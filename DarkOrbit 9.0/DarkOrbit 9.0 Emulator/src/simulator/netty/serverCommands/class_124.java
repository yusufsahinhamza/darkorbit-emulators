package simulator.netty.serverCommands;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_124
        implements ServerCommand {

    public static short const_111 = 1;

    public static short const_1809 = 0;

    public static int ID = 23773;

    public short var_972 = 0;

    public class_124(short param1) {
        this.var_972 = param1;
    }


    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 0;
    }

    public void read(DataInputStream out) {
        try {
            out.readShort();
            out.readShort();
            this.var_972 = out.readShort();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void writeInternal(DataOutputStream out) {
        try {
            out.writeShort(-15419);
            out.writeShort(31577);
            out.writeShort(this.var_972);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}