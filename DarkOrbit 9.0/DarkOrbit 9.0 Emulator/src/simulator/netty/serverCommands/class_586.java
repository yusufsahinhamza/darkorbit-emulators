package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;


public class class_586
        implements ServerCommand {

    public static int ID = 26823;

    public VisualModifierCommand var_1597;

    public String var_1264 = "";

    public class_586(String param1, VisualModifierCommand param2) {
        this.var_1264 = param1;
        this.var_1597 = param2;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 2;
    }

    public void write(DataOutputStream out) {
        try {
            out.writeShort(ID);
            this.writeInternal(out);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void writeInternal(DataOutputStream out) {
        try {
            this.var_1597.write(out);
            out.writeUTF(this.var_1264);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}