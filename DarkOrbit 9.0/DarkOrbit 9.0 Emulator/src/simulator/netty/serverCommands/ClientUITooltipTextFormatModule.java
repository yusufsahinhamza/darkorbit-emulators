package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

// Something connected with text formatting
public class ClientUITooltipTextFormatModule
        implements ServerCommand {

    public static short const_1089 = 3;
    public static short const_234  = 7;
    public static short const_1964 = 6;
    public static short LOCALIZED  = 5;
    public static short PLAIN      = 0;
    public static short const_2514 = 1;
    public static short const_2280 = 2;
    public static short const_2046 = 4;

    public static int   ID       = 29247;
    public        short var_1413 = 0;

    public ClientUITooltipTextFormatModule(short param1) {
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
            out.writeShort(this.var_1413);
            out.writeShort(-7326);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}