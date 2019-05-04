package simulator.netty.serverCommands;

import java.io.DataOutputStream;
import java.io.IOException;

import simulator.netty.ServerCommand;

public class class_683
        implements ServerCommand {

    public static int ID = 13735;

    public double var_2097 = 0;

    public int designId = 0;

    public int var_2434 = 0;

    public int expansionStage = 0;

    public class_683(int param1, double param2, int pDesignId, int pExpansionStage) {
        this.var_2434 = param1;
        this.var_2097 = param2;
        this.designId = pDesignId;
        this.expansionStage = pExpansionStage;
    }

    public int getID() {
        return ID;
    }

    public int method_1005() {
        return 14;
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
            out.writeShort(this.expansionStage);
            out.writeShort(15978);
            out.writeDouble(this.var_2097);
            out.writeShort(this.designId);
            out.writeShort(-25254);
            out.writeShort(this.var_2434);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}